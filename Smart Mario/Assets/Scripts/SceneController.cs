using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is to manage all the scenes in the game
/// </summary>
public class SceneController : MonoBehaviour
{
    private static SceneController instance = null;

    /// <summary>
    /// This is to implement the singleton class principle
    /// This creates a gameObject that contains an instance of this class
    /// </summary>
    /// <returns></returns>
    public static SceneController GetSceneController()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<SceneController>();
        }
        return instance;
    }

    /// <summary>
    /// This is to load the start menu and display it on the screen
    /// </summary>
    public void ToStartMenu()
    {
        SceneManager.LoadScene("Start");
    }

    /// <summary>
    /// This is to load World 1 map and display it on the screen
    /// </summary>
    public void PlayWorld1()
    {
        SceneManager.LoadSceneAsync("World1");
    }
    /// <summary>
    /// This is to load World 2 map and display it on the screen
    /// </summary>
    public void PlayWorld2()
    {
        SceneManager.LoadSceneAsync("World2");
    }

    /// <summary>
    /// This is to load World Selection page and display it on the screen
    /// </summary>
    public void ToWorldSelection()
    {
        SceneManager.LoadScene("WorldSelection");
    }

    /// <summary>
    /// This is to load Customize Character page and display it on the screen
    /// </summary>
    public void ToCustomizeCharacter()
    {
        SceneManager.LoadScene("CustomizeCharacter");
    }

    /// <summary>
    /// This is to load the main menu and display it on the screen
    /// </summary>
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    /// <summary>
    /// This is to load the statistics page and display it on the screen
    /// </summary>
    public void ToStatistics()
    {
        SceneManager.LoadScene("Statistics");
    }

    /// <summary>
    /// This is to load register page and display it on the screen
    /// </summary>
    public void ToCreateAccount()
    {
        SceneManager.LoadScene("CreateAccount");
    }

    /// <summary>
    /// This is to load the login page and display it on the screen
    /// </summary>
    public void ToLogin()
    {
        SceneManager.LoadScene("Login");
    }

    /// <summary>
    /// This is to quit the game
    /// </summary>
    public void ToQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// This is to load the difficulty selection page of selected minigame and display it on the screen
    /// </summary>
    public void ToDifficultySelection()
    {
        SceneManager.LoadScene("SelectDifficulty");
    }
    /// <summary>
    /// This is to load the level selection page of selected minigame and display it on the screen
    /// </summary>
    public void ToLevelSelection()
    {
        SceneManager.LoadScene("SelectLevel");
    }


    #region Minigame 1 Levels

    /// <summary>
    /// This is to load World 1 Stranded Level 1 game and display it on the screen
    /// </summary>
    public void ToWorld1Minigame1Level1()
    {
        SceneManager.LoadScene("World1_Minigame1_Level1");
    }
    /// <summary>
    /// This is to load World 1 Stranded Level 2 game and display it on the screen
    /// </summary>
    public void ToWorld1Minigame1Level2()
    {
        SceneManager.LoadScene("World1_Minigame1_Level2");
    }
    /// <summary>
    /// This is to load World 1 Stranded Level 3 game and display it on the screen
    /// </summary>
    public void ToWorld1Minigame1Level3()
    {
        SceneManager.LoadScene("World1_Minigame1_Level3");
    }

    /// <summary>
    /// This is to load World 1 Stranded Level 4 game and display it on the screen
    /// </summary>
    public void ToWorld1Minigame1Level4()
    {
        SceneManager.LoadScene("World1_Minigame1_Level4");
    }

    /// <summary>
    /// This is to load World 1 Stranded Level 5 game and display it on the screen
    /// </summary>
    public void ToWorld1Minigame1Level5()
    {
        SceneManager.LoadScene("World1_Minigame1_Level5");
    }

    /// <summary>
    /// This is to load World 2 Stranded Level 1 game and display it on the screen
    /// </summary>
    public void ToWorld2Minigame1Level1()
    {
        SceneManager.LoadScene("World2_Minigame1_Level1");
    }

    /// <summary>
    /// This is to load World 2 Stranded Level 2 game and display it on the screen
    /// </summary>
    public void ToWorld2Minigame1Level2()
    {
        SceneManager.LoadScene("World2_Minigame1_Level2");
    }

    /// <summary>
    /// This is to load World 2 Stranded Level 3 game and display it on the screen
    /// </summary>
    public void ToWorld2Minigame1Level3()
    {
        SceneManager.LoadScene("World2_Minigame1_Level3");
    }

    /// <summary>
    /// This is to load World 2 Stranded Level 4 game and display it on the screen
    /// </summary>
    public void ToWorld2Minigame1Level4()
    {
        SceneManager.LoadScene("World2_Minigame1_Level4");
    }

    /// <summary>
    /// This is to load World 2 Stranded Level 5 game and display it on the screen
    /// </summary>
    public void ToWorld2Minigame1Level5()
    {
        SceneManager.LoadScene("World2_Minigame1_Level5");
    }

    #endregion

    #region Minigame 2 Levels

    /// <summary>
    /// This is to load World 1 Matching Cards Level 1 game and display it on the screen
    /// </summary>
    public void ToWorld1Minigame2Level1()
    {
        SceneManager.LoadScene("Matching Cards");
    }

    /// <summary>
    /// This is to load World 1 Matching Cards Level 2 game and display it on the screen
    /// </summary>
    public void ToWorld1Minigame2Level2()
    {
        
    }

    /// <summary>
    /// This is to load World 1 Matching Cards Level 3 game and display it on the screen
    /// </summary>
    public void ToWorld1Minigame2Level3()
    {
        
    }

    /// <summary>
    /// This is to load World 1 Matching Cards Level 4 game and display it on the screen
    /// </summary>
    public void ToWorld1Minigame2Level4()
    {
        
    }

    /// <summary>
    /// This is to load World 1 Matching Cards Level 5 game and display it on the screen
    /// </summary>
    public void ToWorld1Minigame2Level5()
    {
        
    }

    #endregion

    #region Multiplayer

    /// <summary>
    /// This is to load the multiplayer lobby page and display it on the screen
    /// </summary>
    public void ToMultiplayerLobby()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }

    /// <summary>
    /// This is to load the multiplayer challenge room and display it on the screen
    /// </summary>
    public void ToMultiplayerRoom()
    {
        SceneManager.LoadScene("MultiplayerRoom");
    }

    #endregion
}
