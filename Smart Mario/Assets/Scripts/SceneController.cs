using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance = null;

    public static SceneController getSceneController()
    {
        if (instance == null)
        {
            instance = new SceneController();
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
    public void ToMinigame1()
    {
        SceneManager.LoadScene("Minigame1_Level1");
    }

    public void ToMinigame2()
    {
        SceneManager.LoadScene("Matching Cards");
    }
}
