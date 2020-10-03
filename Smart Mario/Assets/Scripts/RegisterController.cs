using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegisterController : MonoBehaviour
{
    //Singleton
    private static RegisterController instance = null;
    private string url = "https://smart-mario-backend-1.herokuapp.com/api/";

    public static RegisterController GetRegisterController()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<RegisterController>();
        }
        return instance;
    }
    public void RegisterTeacherDetails(string username, string password, string school_key, Text msg)
    {
        APICall apiCall = APICall.getAPICall();
        UnityEngine.Debug.Log(username + " " + password + " " + school_key);
        Teacher teacher = new Teacher(username, password, school_key);
        string bodyJsonString = apiCall.saveToJSONString(teacher);
        StartCoroutine(apiCall.RegisterPostRequest(url+"teachers", bodyJsonString, msg));
    }  

    public void RegisterStudentDetails(string username, string password, string teacher_key, Text msg)
    {
        APICall apiCall = APICall.getAPICall();
        UnityEngine.Debug.Log(username + " " + password + " " + teacher_key);
        Student student = new Student(username, password, teacher_key);
        string bodyJsonString = apiCall.saveToJSONString(student);
        StartCoroutine(apiCall.RegisterPostRequest(url + "students", bodyJsonString, msg));
    }
}
