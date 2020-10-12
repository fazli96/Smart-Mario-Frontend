using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class manages the UI elements present when the player is in Minigame Stranded.
/// </summary>
public class StrandedMultiplayerUIManager : MonoBehaviour
{

    public GameObject rulesPanel;
    private SceneController scene;

    /// <summary>
    /// This is called before the first frame update
    /// </summary>
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    /// <summary>
    /// This method is called when the 'Leave Game'button is pressed
    /// It navigates the player back to the multiplayer lobby
    /// </summary>
    public void ToMultiplayerLobby()
    {
        NetworkManager.instance.GetComponent<NetworkManager>().CommandLeaveChallenge();
    }
}
