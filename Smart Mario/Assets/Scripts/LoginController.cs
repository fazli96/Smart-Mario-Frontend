using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class LoginController : MonoBehaviour
{
    //Singleton
    private static LoginController instance = null;
    private string url = "https://smart-mario-backend-1.herokuapp.com/api/";

    public static LoginController GetLoginController()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<LoginController>();
        }
        return instance;
    }

    public void ValidateTeacherLogin(string username, string password)
    {
        APICall apiCall = APICall.getAPICall();
        Teacher teacher = new Teacher(username, password, "");
        string bodyJsonString = apiCall.saveToJSONString(teacher);
        StartCoroutine(apiCall.PostRequest(url + "teachers/authenticate", bodyJsonString));
        UnityEngine.Debug.Log("Continue");
    }

    public void ValidateStudentLogin(string username, string password)
    {
        APICall apiCall = APICall.getAPICall();
        Student student = new Student(username, password, "");
        string bodyJsonString = apiCall.saveToJSONString(student);
        StartCoroutine(apiCall.PostRequest(url + "students/authenticate", bodyJsonString));
    }
}

