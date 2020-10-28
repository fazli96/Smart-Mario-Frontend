using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Controller that connects to Unity Register Scene UI objects and triggers function calls on events
/// </summary>
public class RegisterManager : MonoBehaviour
{
    //Singleton
    public static RegisterManager instance = null;
    private string url = "https://smart-mario-backend-1.herokuapp.com/api/";
    private SceneController scene;
    public Button loginButton;
    public Button createButton;
    public InputField usernameInputField;
    public InputField nameInputField;
    public InputField passwordInputField;
    public InputField keyInputField;
    public Toggle teacherToggle;
    public Text msg;

    /// <summary>
    /// Get instance of SceneController once RegisterManager starts and sets password input to maximum of 15
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        scene = SceneController.GetSceneController();
        passwordInputField.characterLimit = 15;
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Changes scene to Login
    /// </summary>
    public void ToLogin()
    {
        scene.ToLogin();
    }
    /// <summary>
    /// Changes scene to Start Menu
    /// </summary>
    public void ReturnToStartMenu()
    {
        scene.ToStartMenu();
    }
    /// <summary>
    /// Takes username, name, password and key details and sends them for Teacher/Student validation and potential subsequent registration
    /// </summary>
    public void CheckInput()
    {
        string username = usernameInputField.text;
        string name = nameInputField.text;
        string password = passwordInputField.text;
        string key = keyInputField.text;

        if (teacherToggle.isOn)
        {
            if(password.Length>= 5)
            {
                RegisterTeacherDetails(username, name, password, key);
            }
            else
            {
                DisplayMessage("Invalid, password must be between 5-15 characters!");
            }
        }

        else
        {
            if (password.Length >= 5)
            {
                RegisterStudentDetails(username, name, password, key);
            }
            else
            {
                DisplayMessage("Invalid, password must be between 5-15 characters!");
            }
        }
    }
    /// <summary>
    /// Displays error message on screen for failed registration attempts
    /// </summary>
    /// <param name="str"></param>
    /// <param name="message"></param>
    public void DisplayMessage(String str)
    {
        msg.text = str;
    }
    /// <summary>
    ///  Sends entered registration details of Teacher for validation and pontential subsequent registratio
    /// </summary>
    /// <param name="username"></param>
    /// <param name="name"></param>
    /// <param name="password"></param>
    /// <param name="school_key"></param>
    /// <param name="msg"></param>
    private void RegisterTeacherDetails(string username, string name, string password, string school_key)
    {
        APICall apiCall = APICall.getAPICall();
        UnityEngine.Debug.Log(username + " " + name + " " + password + " " + school_key);
        Teacher teacher = new Teacher(username, name, password, school_key);
        string bodyJsonString = apiCall.saveToJSONString(teacher);
        StartCoroutine(apiCall.RegisterPostRequest(url + "teachers", bodyJsonString));
    }
    /// <summary>
    /// Sends entered registration details of Student for validation and pontential subsequent registration 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="name"></param>
    /// <param name="password"></param>
    /// <param name="teacher_key"></param>
    /// <param name="msg"></param>
    private void RegisterStudentDetails(string username, string name, string password, string teacher_key)
    {
        APICall apiCall = APICall.getAPICall();
        UnityEngine.Debug.Log(username + " " + name + " " + password + " " + teacher_key);
        Student student = new Student(username, name, password, teacher_key);
        string bodyJsonString = apiCall.saveToJSONString(student);
        StartCoroutine(apiCall.RegisterPostRequest(url + "students", bodyJsonString));
    }
}
