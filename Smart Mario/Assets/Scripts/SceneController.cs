using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
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

    public void ToMinigame1()
    {
        SceneManager.LoadSceneAsync("SnakesAndLadders");
    }

    public void ToMinigame2()
    {
        SceneManager.LoadSceneAsync("Matching Cards");
    }
}
