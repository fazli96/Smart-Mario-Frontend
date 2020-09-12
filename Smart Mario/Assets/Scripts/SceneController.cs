using System.Collections;
using System.Collections.Generic;
using System.Runtime.Hosting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void PlayWorld1()
    {
        SceneManager.LoadScene("World1");
    }
    public void PlayWorld2()
    {
        SceneManager.LoadScene("World2");
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

    public void Quit()
    {
        Application.Quit();
    }


}
