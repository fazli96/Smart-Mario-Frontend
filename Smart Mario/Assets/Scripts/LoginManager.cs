﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Boundary that connects to Unity Login Scene UI objects and triggers function calls on events
/// </summary>
public class LoginManager : MonoBehaviour
{
    //Singleton
    private static LoginManager instance = null;
    private string url = "https://smart-mario-backend-1.herokuapp.com/api/";
    private SceneController scene;
    public Button loginButton;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Toggle teacherToggle;
    public Text msg;
    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static LoginManager GetLoginManager()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<LoginManager>();
        }
        return instance;
    }
    /// <summary>
    /// Get instances of SceneController once LoginScreen starts
    /// </summary>
    void Start()
    {
        scene = SceneController.GetSceneController();
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
            ValidateTeacherLogin(username, password, msg);
        }
        else
        {
           ValidateStudentLogin(username, password, msg);
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
    public void StudentLoginSuccess()
    {
        scene = SceneController.GetSceneController();
        scene.ToMainMenu();
    }
    /// <summary>
    /// Changes scene to Teacher Menu
    /// </summary>
    public void TeacherLoginSuccess()
    {
        scene = SceneController.GetSceneController();
        scene.ToTeacherMenu();
    }

    /// <summary>
    /// Check the Student login details are valid
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="msg"></param>
    public void ValidateStudentLogin(string username, string password, Text msg)
    {
        APICall apiCall = APICall.getAPICall();
        Student student = new Student(username, "", password, "");
        string bodyJsonString = apiCall.saveToJSONString(student);
        StartCoroutine(apiCall.StudentLoginPostRequest(url + "students/authenticate", bodyJsonString, msg));
    }

    /// <summary>
    /// Check the Teacher login details are valid
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="msg"></param>
    public void ValidateTeacherLogin(string username, string password, Text msg)
    {
        APICall apiCall = APICall.getAPICall();
        Teacher teacher = new Teacher(username, "", password, "");
        string bodyJsonString = apiCall.saveToJSONString(teacher);
        StartCoroutine(apiCall.TeacherLoginPostRequest(url + "teachers/authenticate", bodyJsonString, msg));
    }
}
