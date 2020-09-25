using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.Networking;

public class RegisterController : MonoBehaviour
{
    //Singleton
    private static RegisterController instance = null;
    private string url = "localhost:3000/api/";

    public static RegisterController GetRegisterController()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<RegisterController>();
        }
        return instance;
    }
    public void RegisterTeacherDetails(string username, string password, string teacher_key)
    {
        APICall apiCall = APICall.getAPICall();
        Teacher teacher = new Teacher(username, password, teacher_key);
        string bodyJsonString = apiCall.saveToJSONString(teacher);
        StartCoroutine(apiCall.PostRequest(url+"teachers", bodyJsonString));
    }  

    public void RegisterStudentDetails(string username, string password, string student_key)
    {
        APICall apiCall = APICall.getAPICall();
        Student student = new Student(username, password, student_key);
        string bodyJsonString = apiCall.saveToJSONString(student);
        StartCoroutine(apiCall.PostRequest(url + "students", bodyJsonString));
    }
}
