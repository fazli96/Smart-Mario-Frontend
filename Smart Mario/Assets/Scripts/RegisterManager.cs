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
    private static RegisterManager instance = null;
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
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static RegisterManager GetRegisterManager()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<RegisterManager>();
        }
        return instance;
    }
    /// <summary>
    /// Get instances of SceneController once RegisterManager starts
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
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
            UnityEngine.Debug.Log("Here");
            RegisterTeacherDetails(username, name, password, key, msg);
        }

        else
        {
            RegisterStudentDetails(username, name, password, key, msg);
        }
    }
    /// <summary>
    /// Displays error message on screen for failed registration attempts
    /// </summary>
    /// <param name="str"></param>
    /// <param name="message"></param>
    public void DisplayMessage(String str, Text message)
    {
        this.msg = message;
        msg.text = str;
    }
    private void RegisterTeacherDetails(string username, string name, string password, string school_key, Text msg)
    {
        APICall apiCall = APICall.getAPICall();
        UnityEngine.Debug.Log(username + " " + name + " " + password + " " + school_key);
        Teacher teacher = new Teacher(username, name, password, school_key);
        string bodyJsonString = apiCall.saveToJSONString(teacher);
        StartCoroutine(apiCall.RegisterPostRequest(url + "teachers", bodyJsonString, msg));
    }
    /// <summary>
    /// Sends entered registration details of Student for validation and pontential subsequent registration 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="name"></param>
    /// <param name="password"></param>
    /// <param name="teacher_key"></param>
    /// <param name="msg"></param>
    private void RegisterStudentDetails(string username, string name, string password, string teacher_key, Text msg)
    {
        APICall apiCall = APICall.getAPICall();
        UnityEngine.Debug.Log(username + " " + name + " " + password + " " + teacher_key);
        Student student = new Student(username, name, password, teacher_key);
        string bodyJsonString = apiCall.saveToJSONString(student);
        StartCoroutine(apiCall.RegisterPostRequest(url + "students", bodyJsonString, msg));
    }
}
