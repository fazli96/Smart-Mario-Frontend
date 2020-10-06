using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Boundary that connects to Unity Login Scene UI objects and triggers function calls on events
/// </summary>
public class LoginScreen : MonoBehaviour
{
    //Singleton
    private static LoginScreen instance = null;
    private SceneController scene;
    private LoginController login;
    public Button loginButton;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Toggle teacherToggle;
    public Text msg;
    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static LoginScreen GetLoginScreen()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<LoginScreen>();
        }
        return instance;
    }
    /// <summary>
    /// Get instances of SceneController and LoginController once LoginScreen starts
    /// </summary>
    void Start()
    {
        scene = SceneController.GetSceneController();
        login = LoginController.GetLoginController();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Changes scene to Start Menu
    /// </summary>
    public void ReturnToStartMenu()
    {
        scene.ToStartMenu();
    }
    /// <summary>
    /// Takes username and password details and sends them for Teacher/Student validation
    /// </summary>
    public void CheckInput()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (teacherToggle.isOn)
        {
            UnityEngine.Debug.Log("Teacher");
            login.ValidateTeacherLogin(username, password, msg);
        }
        else
        {
            UnityEngine.Debug.Log("Student");
            login.ValidateStudentLogin(username, password, msg);
        }
    }
    /// <summary>
    /// Displays error message on screen for failed login attempts
    /// </summary>
    /// <param name="str"></param>
    /// <param name="message"></param>
    public void DisplayMessage(String str, Text message)
    {
        this.msg = message;
        msg.text = str;
    }
    /// <summary>
    /// Changes scene to Main Menu
    /// </summary>
    public void LoginSuccess()
    {
        scene = SceneController.GetSceneController();
        scene.ToMainMenu();
    }
}
