using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginController : MonoBehaviour
{
    private static LoginController instance = null;

    public static LoginController GetLoginController()
    {
        if (instance == null)
        {
            instance = new LoginController();
        }
        return instance;
    }

    public Boolean ValidateTeacherLogin(string username, string password)
    {
        //check against database
        return true; //testing
    }

    public Boolean ValidateStudentLogin(string username, string password)
    {
        //check against database
        return true; //testing
    }
}

