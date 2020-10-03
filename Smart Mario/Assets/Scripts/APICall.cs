﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class APICall
{
    //Singleton
    private static APICall instance = null;


    public static APICall getAPICall()
    {
        if (instance == null)
        {
            instance = new APICall();
        }
        return instance;
    }

    public string saveToJSONString(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public IEnumerator RegisterPostRequest(string url, string bodyJsonString, Text msg)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        RegisterScreen regScreen = RegisterScreen.GetRegisterScreen();
        UnityEngine.Debug.Log(request.downloadHandler.text);
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log(convertedStr);

        if (convertedStr.Contains("Error"))
        {
            string errorMsg = convertedStr.Substring(convertedStr.IndexOf(":")+1);
            errorMsg = errorMsg.Substring(0, errorMsg.Length - 1);
            regScreen.DisplayMessage(errorMsg, msg);
        }

        else
        {
            regScreen.DisplayMessage("Successfully created! Please proceed to login. ", msg);
        }
    }

    public IEnumerator LoginPostRequest(string url, string bodyJsonString, Text msg)
    {
        var request = new UnityWebRequest(url, "PT");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        LoginScreen logScreen = LoginScreen.GetLoginScreen(); ;
        UnityEngine.Debug.Log(request.downloadHandler.text);
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log(convertedStr);

        if (convertedStr.Contains("Error"))
        {
            string errorMsg = convertedStr.Substring(convertedStr.IndexOf(":") + 1);
            errorMsg = errorMsg.Substring(0, errorMsg.Length - 1);
            errorMsg = errorMsg.Substring(0, errorMsg.IndexOf(","));
            logScreen.DisplayMessage(errorMsg, msg);
        }

        else
        {
            logScreen.LoginSuccess();
        }
    }

    public IEnumerator ResultsPutRequest(string bodyJsonString)
    {
        var request = new UnityWebRequest("https://smart-mario-backend-1.herokuapp.com/api/results" , "PUT");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log(convertedStr);
    }

    /* public IEnumerator BestResultsGetRequest()
       {
        string url = 
        UnityEngine.Debug.Log(url);
        var request = new UnityWebRequest(url, "GET");
          // byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
         //  request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
           request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
          // request.SetRequestHeader("Content-Type", "application/json");
           request.chunkedTransfer = false;
           yield return request.SendWebRequest();
           string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
           UnityEngine.Debug.Log(convertedStr);
       }*/
    
  /*  public IEnumerator BestResultsGetRequest(string studentID, string minigameID, string difficulty, string level)
    {
        string url = "https://smart-mario-backend-1.herokuapp.com/api/results/:" + studentID + "&:" + minigameID + "&:" + difficulty + "&:" + level;
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                UnityEngine.Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    // handle the result
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                   // var data = (JObject)JsonConvert.DeserializeObject(result);
                    UnityEngine.Debug.Log(result);
                  
                }
                else
                {
                    //handle the problem
                    UnityEngine.Debug.Log("Error! data couldn't get.");
                }
            }
        }
    }*/
    
}


