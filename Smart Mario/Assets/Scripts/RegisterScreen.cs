using System;
using System.Collections;
using System.Collections.Generic;
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
        scene = SceneController.getSceneController();
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
            UnityEngine.Debug.Log("Teacher");
            if (register.CheckTeacherUniqueUsername(username) && register.CheckTeacherValidKey(key))
            {
                // create account in database
                // show successful message
                // transition to next screen
                UnityEngine.Debug.Log("(True set for all tests:) " + username + password + key);
            }
            
            else if (!register.CheckTeacherValidKey(key))
            {
                // show teacher key is invalid message
            }

            else
            {
               // show teacher username is not unique message
            }
        } 

        else
        {
            UnityEngine.Debug.Log("Student");
            if (register.CheckStudentUniqueUsername(username) && register.CheckStudentValidKey(key))
            {
                // create account in database
                // show successful message
                // transition to next screen
                UnityEngine.Debug.Log("(True set for all tests:) " + username + password + key);
            }

            else if (!register.CheckStudentValidKey(key))
            {
                // show student key is invalid message
            }

            else
            {
                // show student username is not unique message
            }
        }
    }
}
