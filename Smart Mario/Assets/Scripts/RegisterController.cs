using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterController : MonoBehaviour
{
    private static RegisterController instance = null;

    public static RegisterController GetRegisterController()
    {
        if (instance == null)
        {
            instance = new RegisterController();
        }
        return instance;
    }

    public void RegisterTeacherDetails(string username, string password)
    {
        //register teacher in database
    }

    public void RegisterStudentDetails(string username, string password)
    {
        //register student in database
    }

    public Boolean CheckTeacherUniqueUsername(string username)
    {
        //check database for duplicate username
        return true; //testing
    }

    public Boolean CheckStudentUniqueUsername(string username)
    {
        //check database for duplicate username
        return true; //testing
    }

    public Boolean CheckTeacherValidKey(string key)
    {
        //check database for valid key
        return true; //testing
    }

    public Boolean CheckStudentValidKey(string key)
    {
        //check database for valid key
        return true; //testing
    }
}
