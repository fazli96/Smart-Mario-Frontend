using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance = null;

    public static SceneController GetSceneController()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<SceneController>();
        }
        return instance;
    }

    public void ToStartMenu()
    {
        SceneManager.LoadScene("Start");
    }

    public void PlayWorld1()
    {
        SceneManager.LoadSceneAsync("World1");
    }
    public void PlayWorld2()
    {
        SceneManager.LoadSceneAsync("World2");
    }

    public void ToWorldSelection()
    {
        SceneManager.LoadScene("WorldSelection");
    }

    public void ToCustomizeCharacter()
    {
        SceneManager.LoadScene("CustomizeCharacter");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ToCreateAccount()
    {
        SceneManager.LoadScene("CreateAccount");
    }

    public void ToLogin()
    {
        SceneManager.LoadScene("Login");
    }

    public void ToQuit()
    {
        Application.Quit();
    }

    public void ToDifficultySelection()
    {
        SceneManager.LoadScene("SelectDifficulty");
    }

    public void ToLevelSelection()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    #region Minigame 1 Levels
    public void ToWorld1Minigame1Level1()
    {
        SceneManager.LoadScene("World1_Minigame1_Level1");
    }

    public void ToWorld1Minigame1Level2()
    {
        SceneManager.LoadScene("World1_Minigame1_Level2");
    }

    public void ToWorld1Minigame1Level3()
    {
        SceneManager.LoadScene("World1_Minigame1_Level3");
    }

    public void ToWorld1Minigame1Level4()
    {
        SceneManager.LoadScene("World1_Minigame1_Level4");
    }
    public void ToWorld1Minigame1Level5()
    {
        SceneManager.LoadScene("World1_Minigame1_Level5");
    }

    #endregion

    #region Minigame 2 Levels
    public void ToWorld1Minigame2Level1()
    {
        SceneManager.LoadScene("Matching Cards");
    }

    public void ToWorld1Minigame2Level2()
    {
        
    }

    public void ToWorld1Minigame2Level3()
    {
        
    }

    public void ToWorld1Minigame2Level4()
    {
        
    }
    public void ToWorld1Minigame2Level5()
    {
        
    }

    #endregion

    #region Multiplayer

    public void ToMultiplayerLobby()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }

    public void ToMultiplayerRoom()
    {
        SceneManager.LoadScene("MultiplayerRoom");
    }

    #endregion
}
