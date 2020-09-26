using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginController : MonoBehaviour
{
    //Singleton
    private static LoginController instance = null;
    private string url = "localhost:3000/api/";

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
    }

    public void ValidateStudentLogin(string username, string password)
    {
        APICall apiCall = APICall.getAPICall();
        Student student = new Student(username, password, "");
        string bodyJsonString = apiCall.saveToJSONString(student);
        StartCoroutine(apiCall.PostRequest(url + "students/authenticate", bodyJsonString));
    }
}

