using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
/// <summary>
/// Controller class for all registration activities performed by Teacher and Student
/// </summary>
public class RegisterController : MonoBehaviour
{
    //Singleton
    private static RegisterController instance = null;
    private string url = "https://smart-mario-backend-1.herokuapp.com/api/";
    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static RegisterController GetRegisterController()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<RegisterController>();
        }
        return instance;
    }
    /// <summary>
    /// Sends entered registration details of Teacher for validation and pontential subsequent registration 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="name"></param>
    /// <param name="password"></param>
    /// <param name="school_key"></param>
    /// <param name="msg"></param>
    public void RegisterTeacherDetails(string username, string name, string password, string school_key, Text msg)
    {
        APICall apiCall = APICall.getAPICall();
        UnityEngine.Debug.Log(username + " " + name + " " + password + " " + school_key);
        Teacher teacher = new Teacher(username, name, password, school_key);
        string bodyJsonString = apiCall.saveToJSONString(teacher);
        StartCoroutine(apiCall.RegisterPostRequest(url+"teachers", bodyJsonString, msg));
    }
    /// <summary>
    /// Sends entered registration details of Student for validation and pontential subsequent registration 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="name"></param>
    /// <param name="password"></param>
    /// <param name="teacher_key"></param>
    /// <param name="msg"></param>
    public void RegisterStudentDetails(string username, string name, string password, string teacher_key, Text msg)
    {
        APICall apiCall = APICall.getAPICall();
        UnityEngine.Debug.Log(username + " " + name + " " + password + " " + teacher_key);
        Student student = new Student(username, name, password, teacher_key);
        string bodyJsonString = apiCall.saveToJSONString(student);
        StartCoroutine(apiCall.RegisterPostRequest(url + "students", bodyJsonString, msg));
    }
}
