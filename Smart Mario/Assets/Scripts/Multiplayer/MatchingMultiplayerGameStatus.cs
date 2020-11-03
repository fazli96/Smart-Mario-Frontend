using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
/// <summary>
/// This class is used for monitoring and managing the player's questions attempted, answered correctly, and computes the end score
/// and updates these results in the database for a Multiplayer game session
/// </summary>
public class MatchingMultiplayerGameStatus : MonoBehaviour
{
    //public Text timeText;
    public GameObject timeText;

    private static float time;
    private static int qnsAttempted;
    private static int qnsAnsweredCorrectly;
    public GameObject saveResultsPanel;
    public Text saveStatusMsg;
    private bool start;

    GameObject GameManager;

    private static readonly string url = "https://smart-mario-backend-1.herokuapp.com/api/results";

    public static MatchingMultiplayerGameStatus instance;

    /// <summary>
    /// This method is called to initialise the singleton class and various variables
    /// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        time = 0;
        start = false;
    }
    /// <summary>
    /// This method is called during every frame update to keep track of the time taken in the game 
    /// It increments the time counter by 2 decimal points unless game is paused
    /// </summary>
    void Update()
    {
        if (start)
        {
            time += Time.deltaTime;
        }
        timeText.GetComponent<UnityEngine.UI.Text>().text = "Time : " + time.ToString("F2");
    }
    /// <summary>
    /// This method is called when the owner starts the multiplayer game 
    /// </summary>
    public void StartGame()
    {
        start = true;
    }
    /// <summary>
    /// This method is called when the Game Status is initialised by the Manager
    /// It resets certain important variables
    /// </summary>
    public void Initialize()
    {
        qnsAttempted = 0;
        qnsAnsweredCorrectly = 0;
        //GameManager = GameObject.Find("GameManager");
        Debug.LogError("found game manager");
    }
    /// <summary>
    /// This method is called when the player matches a pair correctly
    /// </summary>
    public void ScoreIncrease()
    {
        qnsAnsweredCorrectly += 1;
        qnsAttempted += 1;
        Debug.LogError("state : " + qnsAttempted + " " + qnsAnsweredCorrectly);
    }
    /// <summary>
    /// This method is called when the player opens two cards but does not match it correctly
    /// </summary>
    public void QnsAttemptIncrease()
    {
        qnsAttempted += 1;
        Debug.LogError("state : " + qnsAttempted + " " + qnsAnsweredCorrectly);
    }
    /// <summary>
    /// This method is called when the player completes all the pairs
    /// It calls another method to compute the score and store it via a post request
    /// </summary>
    public void EndLevel(bool win)
    {
        start = false;
        ComputeResults(win);
    }
    /// <summary>
    /// This method is called during the winning condition of the game
    /// It assigns two scores based on time taken and accuracy, moderated by the difficulty and level of the game being played
    /// </summary>
    private void ComputeResults(bool win)
    {
        string difficulty = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        int currentLevel = PlayerPrefs.GetInt("MinigameLevel", 1);
        //int timeScore = 0;
        int qnsScore = 0;
        int accScore = 0;
        float acc = qnsAttempted == 0 ? 0 : (float)qnsAnsweredCorrectly / (float)qnsAttempted;

        if (acc == 0)
        {
            accScore = 0;
        }
        else if (acc <= 0.25)
        {
            accScore = 500;
        }
        else if (acc <= 0.5)
        {
            accScore = 1000;
        }
        else if (acc <= 0.75)
        {
            accScore = 1500;
        }
        else accScore = 2000;

        qnsScore = 200 * qnsAnsweredCorrectly;

        //Debug.Log("Time Score is " + timeScore + " and acc score is " + accScore);
        Debug.LogError("Acc Score is " + accScore + " and qns score is " + qnsScore);
        MatchingMultiplayerGameManager.instance.Win(time, qnsAnsweredCorrectly, qnsAttempted, accScore, qnsScore, win);
        SaveResults(difficulty, currentLevel, (accScore + qnsScore));
    }

    /// <summary>
    /// This method saves the results of the game into database after the score has been computed from local variables
    /// </summary>
    /// <param name="difficulty"></param>
    /// <param name="currentLevel"></param>
    /// <param name="score"></param>
    private void SaveResults(string difficulty, int currentLevel, int score)
    {
        int worldSelected = PlayerPrefs.GetInt("World", 1);
        string minigameSelected = PlayerPrefs.GetString("Minigame Selected", "Matching Cards");
        string studentId = PlayerPrefs.GetString("id", "1");
        int minigameId;

        if (worldSelected == 1)
        {
            if (minigameSelected.Equals("Stranded"))
                minigameId = 1;
            else
                minigameId = 2; //minigame 1 is 2 for matching cards world 1
        }
        else
        {
            if (minigameSelected.Equals("Stranded"))
                minigameId = 3;
            else
                minigameId = 4; //minigame 1 is 4 for matching cards world 2
        }
        time = (float)Math.Round(time, 3);
        APICall apiCall = APICall.getAPICall();
        Debug.Log(studentId + ", " + minigameId + ", " + difficulty + ", " + currentLevel + ", " + score + qnsAttempted + ", " + qnsAnsweredCorrectly + ", " + time);
        Results result = new Results(studentId, minigameId, difficulty, currentLevel, score, qnsAttempted, qnsAnsweredCorrectly, time);
        string bodyJsonString = apiCall.saveToJSONString(result);
        StartCoroutine(apiCall.ResultsPutRequest(bodyJsonString));
        Debug.Log("Success");
    }
    /// <summary>
    /// This is to check whether the results is saved to database
    /// </summary>
    /// <param name="result"></param>
    public void ResultsSaved(string result)
    {
        if (result == null)
        {
            ShowResultsNotSaved();
        }
        else
        {
            var data = (JObject)JsonConvert.DeserializeObject(result);
            if (data["success"].ToString() == "True")
            {
                saveResultsPanel.SetActive(false);
            }

            else
            {
                ShowResultsNotSaved();
            }
        }
    }

    /// <summary>
    /// Display warning message to retry or ignore if saving results failed
    /// </summary>
    private void ShowResultsNotSaved()
    {
        saveResultsPanel.SetActive(true);
        saveStatusMsg.text = "Unable to save results.\nClick 'retry' to try again\n\nNote: Clicking Ignore will not save your results";

    }
}
