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
    public Text roomNameText;
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
        roomNameText.text = "Room: " + PlayerPrefs.GetString("roomName", "nil");
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
