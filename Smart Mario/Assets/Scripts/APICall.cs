using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
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
        RegisterManager regManager = RegisterManager.GetRegisterManager();
        UnityEngine.Debug.Log(request.downloadHandler.text);
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log(convertedStr);

        if (convertedStr.Contains("Error"))
        {
            string errorMsg = convertedStr.Substring(convertedStr.IndexOf(":")+1);
            errorMsg = errorMsg.Substring(0, errorMsg.Length - 1);
            regManager.DisplayMessage(errorMsg, msg);
        }

        else
        {
            regManager.DisplayMessage("Successfully created! Please proceed to login. ", msg);
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
        LoginManager logManager = LoginManager.GetLoginManager();
        UnityEngine.Debug.Log(request.downloadHandler.text);
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log(convertedStr);

        if (convertedStr.Contains("Error"))
        {
            string errorMsg = convertedStr.Substring(convertedStr.IndexOf(":") + 1);
            errorMsg = errorMsg.Substring(0, errorMsg.Length - 1);
            errorMsg = errorMsg.Substring(0, errorMsg.IndexOf(","));
            logManager.DisplayMessage(errorMsg, msg);
        }

        else
        {
            logManager.LoginSuccess();
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
        UnityEngine.Debug.Log(convertedStr);
    }
    /// <summary>
    /// Creates an IEnumerator for coroutines that is used for retrieving best game results of Student via a Get request
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public IEnumerator BestResultsGetRequest(string url)
    {
        UnityEngine.Debug.Log(url);
        var request = new UnityWebRequest(url, "GET");
        //byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        //request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log(convertedStr);
        StatisticsManager.instance.GetComponent<StatisticsManager>().ResultsRetrieved(convertedStr);
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
        UnityEngine.Debug.Log("converted" + convertedStr);
        UnityEngine.Debug.Log("Before!");
        if (GameObject.Find("QuestionManager") != null)
        {
            questionManager questionManager = GameObject.Find("QuestionManager").GetComponent<questionManager>();
            UnityEngine.Debug.Log("Success in finding QuestionManager!");
            questionManager.QuestionsRetrieved(convertedStr);
        }
        else if (GameObject.Find("StrandedQuestionManager") != null)
        {
            StrandedQuestionManager.instance.QuestionsRetrieved(convertedStr);
            UnityEngine.Debug.Log("Success in finding StrandedQuestionManager!");
        }
        else
        {
            UnityEngine.Debug.Log("Unable to find questionManager");
        }
    }

    public IEnumerator StudentResultGetRequest(string studentID)
    {
        string url = "https://smart-mario-backend-1.herokuapp.com/api/tasks/student/" + studentID;
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        DisplayListManager displayListManager = DisplayListManager.GetDisplayListManager();
        displayListManager.RetrieveData(convertedStr);
        SceneController scene = SceneController.GetSceneController();
        scene.ToStudentManageTasks();
    }

    public IEnumerator AllStudentResultGetRequest(string teacherID)
    {
        string url = "https://smart-mario-backend-1.herokuapp.com/api/tasks/teacher/" + teacherID;
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log("Getrequest" + convertedStr);
        SelectStudentManager selectStudentManager = SelectStudentManager.GetSelectStudentManager();
        selectStudentManager.CSVRetrieved(convertedStr);
    }

    public IEnumerator SpecificTaskResult(string teacherId, string minigameId, string difficulty, string level)
    {
        string url = "https://smart-mario-backend-1.herokuapp.com/api/tasks/teacher/" + teacherId + "&" + minigameId + "&" + difficulty + "&" + level;
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log("Getrequest" + convertedStr);
        AssignedTasksManager assignedTasksManager = AssignedTasksManager.GetAssignedTasksManager();
        assignedTasksManager.RetrieveData(convertedStr);
        SceneController scene = SceneController.GetSceneController();
        scene.ToViewAssignedTasksScreen();
    }

}


