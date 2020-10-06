using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Boundary that connects to Unity Main Menu Scene UI objects and triggers function calls on events
/// </summary>
public class MainMenuScreen : MonoBehaviour
{
    private static MainMenuScreen instance = null;
    private SceneController scene;
    public Text msg;
    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static MainMenuScreen GetMainMenuScreen()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<MainMenuScreen>();
        }
        return instance;
    }
    /// <summary>
    /// Get instance of SceneController once MainMenuScreen starts and changes welcome according to currently logged in user
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        msg.text = "Welcome " + PlayerPrefs.GetString("username") + "!";
    }

    /// <summary>
    /// Changes scene to Statistics
    /// </summary>
    public void StatisticsScreen()
    {
        scene.ToStatistics();
    }
    /// <summary>
    /// Changes scene to World Selection
    /// </summary>
    public void WorldSelectionScreen()
    {
        scene.ToWorldSelection();
    }

    /// <summary>
    /// Changes scene to Multiplayer Lobby
    /// </summary>
    public void MultiplayerScreen()
    {
        scene.ToMultiplayerLobby();
    }
    /// <summary>
    /// Changes scene to Customize Character
    /// </summary>
    public void CustomizeCharacterScreen()
    {
        scene.ToCustomizeCharacter();
    }
    /// <summary>
    /// Changes scene to Login Screen
    /// </summary>
    public void LogOut()
    {
        scene.ToLogin();
    }

  /*  public void DisplayMessage(string message)
    {
        msg = instance.GetComponent<Text>();
        msg.text = message;
    }*/
}
