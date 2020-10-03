using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScreen : MonoBehaviour
{
    //Singleton
    private static RegisterScreen instance = null;
    private SceneController scene;
    private RegisterController register;
    public Button loginButton;
    public Button createButton;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public InputField keyInputField;
    public Toggle teacherToggle;
    public Text msg;

    public static RegisterScreen GetRegisterScreen()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<RegisterScreen>();
        }
        return instance;
    }
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

    public void ToLogin()
    {
        scene.ToLogin();
    }

    public void ReturnToStartMenu()
    {
        scene.ToStartMenu();
    }

    public void CheckInput()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        string key = keyInputField.text;

        if (teacherToggle.isOn)
        {
            register.RegisterTeacherDetails(username, password, key, msg);
            
    /*        APICall apiCall = APICall.getAPICall();
            Results results = new Results("1", "1", "Easy", "1", "50", "1", "1");
            string bodyJsonString = apiCall.saveToJSONString(results);
            StartCoroutine(apiCall.ResultsPutRequest(bodyJsonString));
            StartCoroutine(apiCall.BestResultsGetRequest("1", "1", "Easy", "1")); */
        } 

        else
        {
            register.RegisterStudentDetails(username, password, key, msg);
        }
    }

    public void DisplayMessage(String str, Text message)
    {
        this.msg = message;
        msg.text = str;
    }

}
