using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Controller class for all login activities performed by Teacher and Student
/// </summary>
public class LoginController : MonoBehaviour
{
    //Singleton
    private static LoginController instance = null;
    private string url = "https://smart-mario-backend-1.herokuapp.com/api/";
/// <summary>
/// Creates a singleton instance if none exist, returns the existing instance if one exists
/// </summary>
/// <returns></returns>
    public static LoginController GetLoginController()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<LoginController>();
        }
        return instance;
    }
    /// <summary>
    /// Checks the Teacher login details are valid
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="msg"></param>
    public void ValidateTeacherLogin(string username, string password, Text msg)
    {
        APICall apiCall = APICall.getAPICall();
        Teacher teacher = new Teacher(username, "", password, "");
        string bodyJsonString = apiCall.saveToJSONString(teacher);
        StartCoroutine(apiCall.LoginPostRequest(url + "teachers/authenticate", bodyJsonString, msg));
    }
    /// <summary>
    /// Check the Student login details are valid
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="msg"></param>
    public void ValidateStudentLogin(string username, string password, Text msg)
    {
        APICall apiCall = APICall.getAPICall();
        Student student = new Student(username, "", password, "");
        string bodyJsonString = apiCall.saveToJSONString(student);
        StartCoroutine(apiCall.LoginPostRequest(url + "students/authenticate", bodyJsonString, msg));
    }
}

