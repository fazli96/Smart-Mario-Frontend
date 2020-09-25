using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class RegisterScreen : MonoBehaviour
{
    private SceneController scene;
    private RegisterController register;

    public Button createButton;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public InputField keyInputField;
    public Toggle teacherToggle;

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
            register.RegisterTeacherDetails(username, password, key);
        } 

        else
        {
            register.RegisterStudentDetails(username, password, key);
        }
    }
}
