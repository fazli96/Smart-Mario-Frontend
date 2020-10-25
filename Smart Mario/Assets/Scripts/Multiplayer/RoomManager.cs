using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class contains all methods that manages the player in the multiplayer room page
/// </summary>
public class RoomManager : MonoBehaviour
{

    public static RoomManager instance;
    public Text roomIDText, roomPasswordText;
    public GameObject startChallengeButton;

    /// <summary>
    /// This method is used for initialization
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
        startChallengeButton.SetActive(false);
    }

    /// <summary>
    /// This method is called to show the room details only when 
    /// the player is connected to the multiplayer server and is in the challenge room
    /// </summary>
    /// <param name="roomID"></param>
    public void ShowRoomDetails(string roomID)
    {
        roomIDText.text = "RoomID: " + roomID;
        string roomPassword = PlayerPrefs.GetString("roomPassword", "");
        if (!roomPassword.Equals(""))
            roomPasswordText.text = "Password: " + roomPassword;
    }

    /// <summary>
    /// This method is called when the player has started a challenge
    /// It navigates all players to the selected minigame
    /// </summary>
    public void StartChallenge()
    {
        NetworkManager.instance.GetComponent<NetworkManager>().StartChallenge();
    }

    /// <summary>
    /// This method is used to enable the Start Challenge button
    /// </summary>
    public void ShowStartChallengeButton()
    {
        startChallengeButton.SetActive(true);
    }

    /// <summary>
    /// This method is used to disable the Start Challenge button
    /// </summary>
    public void DisableStartChallengeButton()
    {
        startChallengeButton.SetActive(false);
    }

    /// <summary>
    /// This method is called when the player leaves the multiplayer challenge room
    /// It navigates the player back to the multiplayer lobby page
    /// </summary>
    public void LeaveRoom()
    {
        NetworkManager.instance.GetComponent<NetworkManager>().CommandLeaveChallenge();
    }
}
