using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour
{
    private SceneController scene;
    private LoginController login;

    public Button loginButton;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Toggle teacherToggle;

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
            login.ValidateTeacherLogin(username, password);
        }
        else
        {
            UnityEngine.Debug.Log("Student");
            login.ValidateStudentLogin(username, password);
        }
    }
}
