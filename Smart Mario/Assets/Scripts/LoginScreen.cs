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
            if (login.ValidateTeacherLogin(username, password))
            {
                // create account in database
                // show successful message
                // transition to next screen
                UnityEngine.Debug.Log("(True set for Teacher Validation:) " + username + password);
            }

            else
            {
                // show teacher username or password not valid
            }
        }

        else
        {
            UnityEngine.Debug.Log("Student");
            if (login.ValidateStudentLogin(username, password))
            {
                // create account in database
                // show successful message
                // transition to next screen
                UnityEngine.Debug.Log("(True set for Student Validation:) " + username + password);
            }

            else
            {
                // show student username or password not valid
            }
        }
    }
}
