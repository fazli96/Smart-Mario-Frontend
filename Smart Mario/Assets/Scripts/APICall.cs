using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
    public IEnumerator TeacherLoginPostRequest(string url, string bodyJsonString, Text msg)
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
            var data = (JObject)JsonConvert.DeserializeObject(convertedStr);
            PlayerPrefs.SetString("username", data["data"]["username"].ToString());
            logManager.TeacherLoginSuccess();
            UnityEngine.Debug.Log(PlayerPrefs.GetString("username"));
        }
    }

    public IEnumerator StudentLoginPostRequest(string url, string bodyJsonString, Text msg)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        LoginManager logManager = LoginManager.GetLoginManager();
        UnityEngine.Debug.Log("here");
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
            var data = (JObject)JsonConvert.DeserializeObject(convertedStr);
            UnityEngine.Debug.Log(data);
            PlayerPrefs.SetString("username", data["data"]["username"].ToString());
            PlayerPrefs.SetString("id", data["data"]["id"].ToString());
            PlayerPrefs.SetString("customChar", data["data"]["custom"].ToString());
            logManager.StudentLoginSuccess();
            //UnityEngine.Debug.Log(PlayerPrefs.GetString("username"));
            //UnityEngine.Debug.Log(PlayerPrefs.GetString("customChar"));
        }       
    }

    public IEnumerator CustomCharacterPutRequest(string id, string custom)
    {

        string url = "https://smart-mario-backend-1.herokuapp.com/api/students/" + id + "&" + custom;
        UnityEngine.Debug.Log(url);
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(saveToJSONString(" "));
        var request = new UnityWebRequest(url, "PUT");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        UnityEngine.Debug.Log("here2");
        yield return request.SendWebRequest();
        UnityEngine.Debug.Log(request.downloadHandler.text);
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log(convertedStr);
        var data = (JObject)JsonConvert.DeserializeObject(convertedStr);
        SceneController scene = SceneController.GetSceneController();
        if (data["success"].ToString() == "True")
        {
            UnityEngine.Debug.Log(data["success"].ToString());
            PlayerPrefs.SetString("customChar", custom);
        }

        else
        {
            UnityEngine.Debug.Log(data["success"].ToString());
        }
        scene.ToMainMenu();
    }

    public IEnumerator AssignTaskPutRequest(string teacherId, string minigameId, string difficulty, string level)
    {
        string url = "https://smart-mario-backend-1.herokuapp.com/api/tasks/" + teacherId + "&" + minigameId + "&" + difficulty + "&" + level;
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(saveToJSONString(" "));
        var request = new UnityWebRequest(url, "PUT");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        var data = (JObject)JsonConvert.DeserializeObject(convertedStr);
        AssignTask assignTask = AssignTask.GetAssignTask();
        assignTask.setRefresh();
        string boolstring = data["success"].ToString();
        assignTask.setSuccessStatus(boolstring == "True");
        SceneController scene = SceneController.GetSceneController();
        scene.ToAssignTasksScreen();
    }

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used for logging game results for Students via a Put request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="bodyJsonString"></param>
    /// <returns></returns>
    public IEnumerator ResultsPutRequest(string bodyJsonString, string url = "https://smart-mario-backend-1.herokuapp.com/api/results")
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
    public IEnumerator BestResultsGetRequest(string studentId, string minigameId, string difficulty, string level)
    {
        string url = "https://smart-mario-backend-1.herokuapp.com/api/results/" + studentId + "&" + minigameId + "&" + difficulty + "&" + level;
        var request = new UnityWebRequest(url, "GET");
        //byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        //request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log(convertedStr);
        StudentPerformanceManager.instance.GetComponent<StudentPerformanceManager>().ResultsRetrieved(convertedStr);
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
        UnityEngine.Debug.Log("Before!");
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log(convertedStr);
        UnityEngine.Debug.Log("After!");
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
        UnityEngine.Debug.Log(url);
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

    public IEnumerator AllStudentResultGetRequest(string teacherID, int sceneNumber)
    {
        string url = "https://smart-mario-backend-1.herokuapp.com/api/tasks/teacher/" + teacherID;
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        DisplayListManager displayListManager = DisplayListManager.GetDisplayListManager();
        displayListManager.RetrieveData(convertedStr);
        SceneController scene = SceneController.GetSceneController();
        switch (sceneNumber)
        {
            case 0:
                SelectStudentManager selectStudentManager = SelectStudentManager.GetSelectStudentManager();
                selectStudentManager.CSVGetRequest(convertedStr);
                scene.ToSelectStudentPerformance();
                break;
            case 1:
                scene.ToTeacherSelectTaskScreen();
                break;
        }
    }

    public IEnumerator SpecificTaskResult(string teacherId, string minigameId, string difficulty, string level)
    {
        string url = "https://smart-mario-backend-1.herokuapp.com/api/tasks/teacher/" + teacherId + "&" + minigameId + "&" + difficulty + "&" + level;
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        AssignedTasksManager assignedTasksManager = AssignedTasksManager.GetAssignedTasksManager();
        assignedTasksManager.RetrieveData(convertedStr);
        SceneController scene = SceneController.GetSceneController();
        scene.ToViewAssignedTasksScreen();
    }

    public IEnumerator LeaderboardGetRequest(string url)
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
        LeaderboardManager.instance.GetComponent<LeaderboardManager>().LeaderboardRetrieved(convertedStr);
    }

}


