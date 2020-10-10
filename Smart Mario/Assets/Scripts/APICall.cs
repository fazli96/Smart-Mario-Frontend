using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
/// <summary>
/// Control that offers consolidated API calls to database
/// </summary>
public class APICall
{
    //Singleton
    private static APICall instance = null;

    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static APICall getAPICall()
    {
        if (instance == null)
        {
            instance = new APICall();
        }
        return instance;
    }
    /// <summary>
    /// Converts object to Json form
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public string saveToJSONString(object obj)
    {
        return JsonUtility.ToJson(obj);
    }
    /// <summary>
    /// Creates an IEnumerator for coroutines that is used for registration of Teachers and Students via a POST request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="bodyJsonString"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Creates an IEnumerator for coroutines that is used for login of Teachers and Students via a POST request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="bodyJsonString"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    public IEnumerator LoginPostRequest(string url, string bodyJsonString, Text msg)
    {
        var request = new UnityWebRequest(url, "POST");
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
            int startIndex = convertedStr.IndexOf("username") + 11;
            string username = convertedStr.Substring(startIndex);
            int len = username.IndexOf('"');
            username = username.Substring(0, len);
            PlayerPrefs.SetString("username", username);
        }
    }
    /// <summary>
    /// Creates an IEnumerator for coroutines that is used for logging game results for Students via a Put request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="bodyJsonString"></param>
    /// <returns></returns>
    public IEnumerator ResultsPutRequest(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url , "PUT");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        Debug.Log(convertedStr);
    }
    /// <summary>
    /// Creates an IEnumerator for coroutines that is used for retrieving best game results of Student via a Get request
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public IEnumerator BestResultsGetRequest(string url)
    {
        Debug.Log(url);
        var request = new UnityWebRequest(url, "GET");
        //byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        //request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        Debug.Log(convertedStr);
        StatisticsController.instance.GetComponent<StatisticsController>().ResultsRetrieved(convertedStr);
    }
    /// <summary>
    /// Creates an IEnumerator for coroutines that is used for retrieving game questions for Student via a Get request
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public IEnumerator AllQuestionsGetRequest(string url)
    { 
        UnityEngine.Debug.Log(url);
        var request = new UnityWebRequest(url, "GET");
        // byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        //  request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        // request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        Debug.Log("converted" + convertedStr);
        Debug.Log("Before!");
        if (GameObject.Find("QuestionManager") != null)
        {
            questionManager questionManager = GameObject.Find("QuestionManager").GetComponent<questionManager>();
            Debug.Log("Success in finding QuestionManager!");
            questionManager.QuestionsRetrieved(convertedStr);
        }
        else if (GameObject.Find("QuestionController") != null)
        {
            QuestionController questionController = GameObject.Find("QuestionController").GetComponent<QuestionController>();
            questionController.QuestionsRetrieved(convertedStr);
            Debug.Log("Success in finding QuestionController!");
        }
        
        Debug.Log("Unable to find questionController");
    }


}


