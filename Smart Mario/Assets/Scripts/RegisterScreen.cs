using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Boundary that connects to Unity Register Scene UI objects and triggers function calls on events
/// </summary>
public class RegisterScreen : MonoBehaviour
{
    //Singleton
    private static RegisterScreen instance = null;
    private SceneController scene;
    private RegisterController register;
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
    public static RegisterScreen GetRegisterScreen()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<RegisterScreen>();
        }
        return instance;
    }
    /// <summary>
    /// Get instances of SceneController and LoginController once RegisterScreen starts
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        register = RegisterController.GetRegisterController();
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
            register.RegisterTeacherDetails(username, name, password, key, msg);
            
    /*        APICall apiCall = APICall.getAPICall();
            Results results = new Results("1", "1", "Easy", "1", "50", "1", "1");
            string bodyJsonString = apiCall.saveToJSONString(results);
            StartCoroutine(apiCall.ResultsPutRequest(bodyJsonString));
            StartCoroutine(apiCall.BestResultsGetRequest("1", "1", "Easy", "1")); */
        } 

        else
        {
            register.RegisterStudentDetails(username, name, password, key, msg);
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

}
