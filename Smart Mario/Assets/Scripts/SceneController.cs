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

    #region Minigame 1 Levels
    public void ToMinigame1Level1()
    {
        SceneManager.LoadScene("Minigame1_Level1");
    }

    public void ToMinigame1Level2()
    {
        
    }

    public void ToMinigame1Level3()
    {
        
    }

    public void ToMinigame1Level4()
    {
        
    }
    public void ToMinigame1Level5()
    {
        
    }

    #endregion

    #region Minigame 2 Levels
    public void ToMinigame2Level1()
    {
        SceneManager.LoadScene("Matching Cards");
    }

    public void ToMinigame2Level2()
    {
        
    }

    public void ToMinigame2Level3()
    {
        
    }

    public void ToMinigame2Level4()
    {
        
    }
    public void ToMinigame2Level5()
    {
        
    }

    #endregion
}
