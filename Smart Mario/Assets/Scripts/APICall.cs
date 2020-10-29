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
    public IEnumerator RegisterPostRequest(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        UnityEngine.Debug.Log(request.downloadHandler.text);
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        var data = (JObject)JsonConvert.DeserializeObject(convertedStr);
        if (convertedStr.Contains("Error"))
        {
            RegisterManager.instance.DisplayMessage(data["message"].ToString());
        }

        else
        {
            RegisterManager.instance.DisplayMessage("Successfully created! Please proceed to login. ");
        }
    }
    /// <summary>
    /// Creates an IEnumerator for coroutines that is used for login of Teachers via a POST request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="bodyJsonString"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    public IEnumerator TeacherLoginPostRequest(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        var data = (JObject)JsonConvert.DeserializeObject(convertedStr);

        if (convertedStr.Contains("Error"))
        {
            LoginManager.instance.DisplayMessage(data["message"].ToString());
        }

        else
        {
            PlayerPrefs.SetString("username", data["data"]["username"].ToString());
            PlayerPrefs.SetString("teacherId", data["data"]["id"].ToString());
            LoginManager.instance.TeacherLoginSuccess();
        }
    }
    /// <summary>
    ///  Creates an IEnumerator for coroutines that is used for login of Teachers via a POST request
    /// </summary>
    /// <param name="url"></param>
    /// <param name="bodyJsonString"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    public IEnumerator StudentLoginPostRequest(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        var data = (JObject)JsonConvert.DeserializeObject(convertedStr);

        if (convertedStr.Contains("Error"))
        {
            LoginManager.instance.DisplayMessage(data["message"].ToString());
        }

        else
        {
            PlayerPrefs.SetString("username", data["data"]["username"].ToString());
            PlayerPrefs.SetString("id", data["data"]["id"].ToString());
            PlayerPrefs.SetString("customChar", data["data"]["custom"].ToString());
            LoginManager.instance.GetQuestions();
        }       
    }
    /// <summary>
    ///  Creates an IEnumerator for coroutines that is used for updating custsomisation of characters via a PUT request
    /// </summary>
    /// <param name="id"></param>
    /// <param name="custom"></param>
    /// <returns></returns>
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
    /// <summary>
    ///  Creates an IEnumerator for coroutines that is used for task assginment for a teachId, minigameId, difficulty and level via a PUT request
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="minigameId"></param>
    /// <param name="difficulty"></param>
    /// <param name="level"></param>
    /// <returns></returns>
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
        AssignTaskManager assignTaskManager = AssignTaskManager.GetAssignTaskManager();
        assignTaskManager.setRefresh();
        string boolstring = data["success"].ToString();
        assignTaskManager.setSuccessStatus(boolstring == "True");
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
        UnityEngine.Debug.Log(bodyJsonString);
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log(convertedStr);
        if (GameObject.Find("StrandedGameStatus") != null)
            StrandedGameStatus.instance.ResultsSaved(convertedStr);
        else if (GameObject.Find("StrandedMultiplayerGameStatus") != null)
            StrandedMultiplayerGameStatus.instance.ResultsSaved(convertedStr);
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
        UnityEngine.Debug.Log("Before!");
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log(convertedStr);
        UnityEngine.Debug.Log("After!");
        //if (GameObject.Find("QuestionManager") != null)
        //{
        //    //questionManager questionManager = GameObject.Find("QuestionManager").GetComponent<questionManager>();
        //    UnityEngine.Debug.Log("Success in finding QuestionManager!");
        //    //questionManager.QuestionsRetrieved(convertedStr);
        //}
        //else if (GameObject.Find("StrandedQuestionManager") != null)
        //{
        //    //StrandedQuestionManager.instance.QuestionsRetrieved(convertedStr);
        //    UnityEngine.Debug.Log("Success in finding StrandedQuestionManager!");
        //}
        //else
        //{
        //    UnityEngine.Debug.Log("Unable to find questionManager");
        //}
    }

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used for retrieving game questions for Student via a Get request
    /// </summary>
    /// <param name="urlExtension"></param>
    /// <returns></returns>
    public IEnumerator QuestionsListGetRequest(string urlExtension)
    {
        string url = "https://smart-mario-backend-1.herokuapp.com/api/questions/" + urlExtension;
        UnityEngine.Debug.Log(url);
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.chunkedTransfer = false;
        UnityEngine.Debug.Log("Before!");
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        UnityEngine.Debug.Log(convertedStr);
        UnityEngine.Debug.Log("After!");

        if (convertedStr == null || convertedStr.Equals("[]"))
        {
            string errorMsg = "Unable to load data from server";
            LoginManager.instance.DisplayMessage(errorMsg);
        }
        else {
            switch (urlExtension)
            {
                case "mcqtheory":
                    LoginManager.instance.McqTheoryQuestionsRetrieved(convertedStr);
                    break;
                case "mcqcode":
                    LoginManager.instance.McqCodeQuestionsRetrieved(convertedStr);
                    break;
                case "shorttheory":
                    LoginManager.instance.ShortTheoryQuestionsRetrieved(convertedStr);
                    break;
                case "shortcode":
                    LoginManager.instance.ShortCodeQuestionsRetrieved(convertedStr);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used for retrieving all statistics for a student via a Get request
    /// </summary>
    /// <param name="urlExtension"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used for retrieving results for all students under a teacher via a Get request
    /// </summary>
    /// <param name="urlExtension"></param>
    /// <returns></returns>
    public IEnumerator AllStudentResultGetRequest(string teacherID, bool sceneSelectStudent)
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
        if (sceneSelectStudent)
        {
            scene.ToSelectStudentPerformance();
        }
        else
        {
            scene.ToTeacherSelectTaskScreen();
        }
    }
    /// <summary>
    ///  Creates an IEnumerator for coroutines that is used for exporting of CSV results for all students tagged to a teacher via a GET request
    /// </summary>
    /// <param name="teacherId"></param>
    /// <returns></returns>
    public IEnumerator CSVExportGetRequest(string teacherId)
    {
        string url = "https://smart-mario-backend-1.herokuapp.com/api/results/teacher/" + teacherId;
        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.chunkedTransfer = false;
        yield return request.SendWebRequest();
        string convertedStr = Encoding.UTF8.GetString(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        SelectStudentManager selectStudentManager = SelectStudentManager.GetSelectStudentManager();
        selectStudentManager.SetRefreshAndExportCSV(convertedStr);
        SceneController scene = SceneController.GetSceneController();
        scene.ToSelectStudentPerformance();
    }
    /// <summary>
    ///  Creates an IEnumerator for coroutines that is used for getting task result for a specific task tagged to a teacher, minigamedID, difficulty and level via a GET request
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="minigameId"></param>
    /// <param name="difficulty"></param>
    /// <param name="level"></param>
    /// <returns></returns>
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
    /// <summary>
    ///  Creates an IEnumerator for coroutines that is used for leaderboard standings via a GET request
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
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

    /// <summary>
    ///  Creates an IEnumerator for coroutines that is used for retrieving student's ranking in a leaderboard via a GET request
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public IEnumerator StudentLeaderboardRankGetRequest(string url)
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
        LeaderboardManager.instance.GetComponent<LeaderboardManager>().StudentLeaderboardRankRetrieved(convertedStr);
    }

}


