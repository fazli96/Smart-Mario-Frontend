using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public static LoginScreen GetLoginScreen()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<LoginScreen>();
        }
        return instance;
    }

    void Start()
    {
        scene = SceneController.GetSceneController();
        login = LoginController.GetLoginController();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReturnToStartMenu()
    {
        scene.ToStartMenu();
    }
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

    public void DisplayMessage(String str, Text message)
    {
        this.msg = message;
        msg.text = str;
    }

    public void LoginSuccess()
    {
        scene = SceneController.GetSceneController();
        scene.ToMainMenu();
    }
}
