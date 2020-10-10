using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

/// <summary>
/// This class contains all method that manages the user in the Multiplayer Lobby page
/// </summary>
public class LobbyManager : MonoBehaviour
{

    public static LobbyManager instance;
    private SceneController scene;

    public InputField roomNameInput;
    public InputField roomCapacityInput;
    public InputField playerNameInputCreate;
    public Dropdown minigameSelectedDropdown;
    public Dropdown difficultySelectedDropdown;

    public Text roomIDText;
    public Text roomNameText;
    public Text roomOwnerText;
    public Text roomCapacityText;
    public Text minigameSelectedText;
    public Text difficultySelectedText;
    public InputField playerNameInputJoin;
    public Dropdown roomSelectedDropdown;
    public GameObject joinButton;

    public Text errorText;
    public GameObject errorPanel;
    private static List<RoomJSON> rooms;

    /// <summary>
    /// This method is called for initialization
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        scene = SceneController.GetSceneController();
        errorPanel.SetActive(false);
        roomSelectedDropdown.options.Clear();
        rooms = new List<RoomJSON>();
        joinButton.SetActive(false);
    }


    /// <summary>
    /// This method is called when 'Back' button is called
    /// It navigates the player back to the main menu
    /// </summary>
    public void BackToMainMenu()
    {
        scene.ToMainMenu();
    }

    /// <summary>
    /// This method is called when the challenge room is full
    /// </summary>
    public void RoomIsFull()
    {
        errorText.text = "Room is full";
        errorPanel.SetActive(true);
    }

    /// <summary>
    /// This method is called when a player has created a multiplayer challenge room 
    /// </summary>
    public void CreateRoom()
    {
        string errorMsg = ValidateFields(roomNameInput.text, roomCapacityInput.text, playerNameInputCreate.text);
        if (errorMsg.Equals("success"))
        {
            PlayerPrefs.SetString("roomName", roomNameInput.text);
            PlayerPrefs.SetInt("roomCapacity", int.Parse(roomCapacityInput.text));
            PlayerPrefs.SetString("username", playerNameInputCreate.text);
            PlayerPrefs.SetString("roomID", "create");
            switch (minigameSelectedDropdown.value)
            {
                case 0:
                    PlayerPrefs.SetString("Minigame Selected", "World 1 Stranded");
                    break;
                case 1:
                    PlayerPrefs.SetString("Minigame Selected", "World 1 Matching Cards");
                    break;
                case 2:
                    PlayerPrefs.SetString("Minigame Selected", "World 2 Stranded");
                    break;
                case 3:
                    PlayerPrefs.SetString("Minigame Selected", "World 2 Matching Cards");
                    break;
                default:
                    break;
            }
            switch (difficultySelectedDropdown.value)
            {
                case 0:
                    PlayerPrefs.SetString("Difficulty Selected", "Easy");
                    break;
                case 1:
                    PlayerPrefs.SetString("Difficulty Selected", "Medium");
                    break;
                case 2:
                    PlayerPrefs.SetString("Difficulty Selected", "Hard");
                    break;
                default:
                    break;
            }
            NetworkManager.instance.GetComponent<NetworkManager>().JoinRoom();
        }
        else
        {
            errorText.text = errorMsg;
            errorPanel.SetActive(true);
        }
    }

    /// <summary>
    /// This method is called when the player has joined a challenge room
    /// </summary>
    public void JoinRoom()
    {
        int index = roomSelectedDropdown.value;
        PlayerPrefs.SetString("roomName", rooms[index].roomName);
        PlayerPrefs.SetInt("roomCapacity", rooms[index].roomCapacity);
        PlayerPrefs.SetString("username", playerNameInputJoin.text);
        PlayerPrefs.SetString("roomID", rooms[index].roomID);
        PlayerPrefs.SetString("Minigame Selected", rooms[index].minigameSelected);
        PlayerPrefs.SetString("Difficulty Selected", rooms[index].difficultySelected);
        NetworkManager.instance.GetComponent<NetworkManager>().JoinRoom();
    }

    /// <summary>
    /// This method is called to validate the fields in the create challenge form
    /// </summary>
    /// <param name="roomName"></param>
    /// <param name="roomCapacity"></param>
    /// <param name="playerName"></param>
    /// <returns></returns>
    private string ValidateFields(string roomName, string roomCapacity, string playerName)
    {
        if (roomName.Equals("") || roomCapacity.Equals("") || playerName.Equals(""))
        {
            return "Room Name/Room Capacity/Player Name cannot be empty";
        }
        bool isNumeric = int.TryParse(roomCapacity, out int roomCapacityInt);
        if (!isNumeric)
        {
            return "Room capacity must be an integer";
        }
        if (roomCapacityInt < 2 || roomCapacityInt > 4)
        {
            return "Room capacity must be between 2-4";
        }
        return "success";

    }

    /// <summary>
    /// This method is called to retrieve available rooms from the game server
    /// </summary>
    public void GetRooms()
    {
        NetworkManager.instance.GetComponent<NetworkManager>().CommandGetRooms();
    }

    /// <summary>
    /// This method is called when different challenge room is selected from the dropdown bar
    /// </summary>
    public void OnRoomSelectedValueChange()
    {
        int index = roomSelectedDropdown.value;
        roomIDText.text = "Room ID: " + rooms[index].roomID;
        roomNameText.text = "Room Name: " + rooms[index].roomName;
        roomOwnerText.text = "Room Owner: " + rooms[index].roomOwner;
        roomCapacityText.text = "Room Capacity: " + rooms[index].noOfClients.ToString() + '/' + rooms[index].roomCapacity;
        minigameSelectedText.text = "Minigame Selected: " + rooms[index].minigameSelected;
        difficultySelectedText.text = "Difficulty Selected: " + rooms[index].difficultySelected;
    }

    /// <summary>
    /// This method is called to display available rooms in the form of a dropdown bar
    /// </summary>
    /// <param name="data"></param>
    public void AddRooms(JObject data)
    {
        JArray roomJArray = data["rooms"].Value<JArray>();
        if (roomJArray.Count != 0)
        {
            foreach (JObject roomObject in roomJArray)
            {
                Debug.Log("room: " + roomObject);
                RoomJSON room = RoomJSON.CreateFromJSON(roomObject.ToString());
                Debug.Log(room.roomName);
                if (room.noOfClients < room.roomCapacity)
                {
                    rooms.Add(room);
                    roomSelectedDropdown.options.Add(new Dropdown.OptionData() { text = room.roomName });
                }
                
            };
            roomSelectedDropdown.value = 0;
            roomSelectedDropdown.RefreshShownValue();
            roomIDText.text = "Room ID: " + rooms[0].roomID;
            roomNameText.text = "Room Name: " + rooms[0].roomName;
            roomOwnerText.text = "Room Owner: " + rooms[0].roomOwner;
            roomCapacityText.text = "Room Capacity: " + rooms[0].noOfClients + '/' + rooms[0].roomCapacity;
            minigameSelectedText.text = "Minigame Selected: " + rooms[0].minigameSelected;
            difficultySelectedText.text = "Difficulty Selected: " + rooms[0].difficultySelected;
            joinButton.SetActive(true);
        }
        else
        {
            roomIDText.text = "No rooms available to join";
            joinButton.SetActive(false);
        }
        
    }

    /// <summary>
    /// This class is to store the room retrieved from the server
    /// </summary>
    [Serializable]
    public class RoomJSON
    {
        public string roomID;
        public string roomName;
        public string roomOwner;
        public int noOfClients;
        public int roomCapacity;
        public string minigameSelected;
        public string difficultySelected;

        /// <summary>
        /// This method is to instantiate a class based on a jsonString
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RoomJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<RoomJSON>(data);
        }
    }



}