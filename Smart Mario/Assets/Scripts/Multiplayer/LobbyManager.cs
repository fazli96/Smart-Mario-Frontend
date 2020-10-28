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
    public InputField passwordInputCreate;
    public Dropdown minigameSelectedDropdown;
    public Dropdown difficultySelectedDropdown;
    public Dropdown levelSelectedDropdown;

    public Text roomNameText;
    public Text roomOwnerText;
    public Text roomCapacityText;
    public Text roomPasswordText;
    public Text minigameSelectedText;
    public Text difficultySelectedText;
    public Text levelSelectedText;
    public GameObject passwordInputJoinObject;
    public InputField passwordInputJoin;
    public Dropdown roomSelectedDropdown;
    public GameObject joinButton;

    public Text errorText;
    public GameObject errorPanel;
    private static List<RoomJSON> rooms;

    /// <summary>
    /// This method is called for initialization to get instance of Scene Controller and initialize the inputfields to appropriate restrictions
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
        roomNameInput.characterLimit = 15;
        roomCapacityInput.characterLimit = 1;
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
    /// This method is called when the player enters the wrong password when joining a challenge room
    /// </summary>
    public void WrongRoomPassword()
    {
        errorText.text = "Room password entered is incorrect";
        errorPanel.SetActive(true);
    }

    /// <summary>
    /// This method is called when the challenge has not enough players
    /// Happens only when the player is in the midst of playing the minigame selected in the challenge
    /// </summary>
    public void NotEnoughPlayers()
    {
        errorText.text = "Not enough players";
        errorPanel.SetActive(true);
    }

    /// <summary>
    /// This method is called when a player has created a multiplayer challenge room 
    /// </summary>
    public void CreateRoom()
    {
        string errorMsg = ValidateFields(roomNameInput.text, roomCapacityInput.text, passwordInputCreate.text);
        if (errorMsg.Equals("success"))
        {
            PlayerPrefs.SetString("roomName", roomNameInput.text);
            PlayerPrefs.SetInt("roomCapacity", int.Parse(roomCapacityInput.text));
            PlayerPrefs.SetString("roomPassword", passwordInputCreate.text);
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
                    PlayerPrefs.SetString("Minigame Difficulty", "Easy");
                    break;
                case 1:
                    PlayerPrefs.SetString("Minigame Difficulty", "Medium");
                    break;
                case 2:
                    PlayerPrefs.SetString("Minigame Difficulty", "Hard");
                    break;
                default:
                    break;
            }
            switch (levelSelectedDropdown.value)
            {
                case 0:
                    PlayerPrefs.SetInt("MinigameLevel", 1);
                    break;
                case 1:
                    PlayerPrefs.SetInt("MinigameLevel", 2);
                    break;
                case 2:
                    PlayerPrefs.SetInt("MinigameLevel", 3);
                    break;
                case 3:
                    PlayerPrefs.SetInt("MinigameLevel", 4);
                    break;
                case 4:
                    PlayerPrefs.SetInt("MinigameLevel", 5);
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
        PlayerPrefs.SetString("roomPassword", passwordInputJoin.text);
        PlayerPrefs.SetString("roomID", rooms[index].roomID);
        PlayerPrefs.SetString("Minigame Selected", rooms[index].minigameSelected);
        PlayerPrefs.SetString("Minigame Difficulty", rooms[index].difficultySelected);
        PlayerPrefs.SetInt("MinigameLevel", rooms[index].levelSelected);
        NetworkManager.instance.GetComponent<NetworkManager>().JoinRoom();
    }

    /// <summary>
    /// This method is called to validate the fields in the create challenge form
    /// </summary>
    /// <param name="roomName"></param>
    /// <param name="roomCapacity"></param>
    /// <param name="playerName"></param>
    /// <returns></returns>
    private string ValidateFields(string roomName, string roomCapacity, string roomPassword)
    {
        if (roomName.Equals("") || roomCapacity.Equals(""))
        {
            return "Room Name/Room Capacity/Player Name cannot be empty";
        }
        else if (roomName.Length < 5)
        {
            return "Room Name must be at least 5 characters long";
        }
        else if (!roomPassword.Equals("") && roomPassword.Length < 5)
        {
            return "Room Password must be at least 5 characters long";
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
        rooms.Clear();
        NetworkManager.instance.GetComponent<NetworkManager>().CommandGetRooms();
    }

    /// <summary>
    /// This method is called when different challenge room is selected from the dropdown bar
    /// </summary>
    public void OnRoomSelectedValueChange()
    {
        int index = roomSelectedDropdown.value;
        roomNameText.text = "Room Name: " + rooms[index].roomName;
        roomOwnerText.text = "Room Owner: " + rooms[index].roomOwner;
        roomCapacityText.text = "Room Capacity: " + rooms[index].noOfClients.ToString() + '/' + rooms[index].roomCapacity;
        if (rooms[index].roomPassword.Equals(""))
        {
            roomPasswordText.text = "Room Type: Public";
            passwordInputJoinObject.SetActive(false);
        }
        else
        {
            roomPasswordText.text = "Room Type: Private";
            passwordInputJoinObject.SetActive(true);
        }
        minigameSelectedText.text = "Minigame Selected: " + rooms[index].minigameSelected;
        difficultySelectedText.text = "Difficulty Selected: " + rooms[index].difficultySelected;
        levelSelectedText.text = "Level Selected: " + rooms[index].levelSelected;
        passwordInputJoin.text = "";
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
                RoomJSON room = RoomJSON.CreateFromJSON(roomObject.ToString());
                Debug.Log(room.roomID);
                if (room.noOfClients < room.roomCapacity)
                {
                    rooms.Add(room);
                    roomSelectedDropdown.options.Add(new Dropdown.OptionData() { text = "RoomID: " + room.roomID });
                }
                
            };
            roomSelectedDropdown.value = 0;
            roomSelectedDropdown.RefreshShownValue();
            roomNameText.text = "Room Name: " + rooms[0].roomName;
            roomOwnerText.text = "Room Owner: " + rooms[0].roomOwner;
            roomCapacityText.text = "Room Capacity: " + rooms[0].noOfClients + '/' + rooms[0].roomCapacity;
            if (rooms[0].roomPassword.Equals(""))
            {
                roomPasswordText.text = "Room Type: Public";
                passwordInputJoinObject.SetActive(false);
            }
            else
            {
                roomPasswordText.text = "Room Type: Private";
                passwordInputJoinObject.SetActive(true);
            }
            minigameSelectedText.text = "Minigame Selected: " + rooms[0].minigameSelected;
            difficultySelectedText.text = "Difficulty Selected: " + rooms[0].difficultySelected;
            levelSelectedText.text = "Level Selected: " + rooms[0].levelSelected;
            joinButton.SetActive(true);
            passwordInputJoin.text = "";
        }
        else
        {
            roomSelectedDropdown.options.Clear();
            roomSelectedDropdown.RefreshShownValue();
            roomNameText.text = "No rooms available to join";
            roomOwnerText.text = "";
            roomCapacityText.text = "";
            roomPasswordText.text = "";
            minigameSelectedText.text = "";
            difficultySelectedText.text = "";
            levelSelectedText.text = "";
            passwordInputJoinObject.SetActive(false);
            joinButton.SetActive(false);
        }
        
    }

    /// <summary>
    /// This class is to store the challenge room retrieved from the server
    /// </summary>
    [Serializable]
    public class RoomJSON
    {
        public string roomID;
        public string roomName;
        public string roomOwner;
        public int noOfClients;
        public int roomCapacity;
        public string roomPassword;
        public string minigameSelected;
        public string difficultySelected;
        public int levelSelected;

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