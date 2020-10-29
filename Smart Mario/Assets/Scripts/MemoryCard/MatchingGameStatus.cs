using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;


/// <summary>
/// /// This class is used for monitoring and managing the player's score, questions attempted and questions answered correctly
/// and updating these results in the database in Single Player game session
/// </summary>
public class MatchingGameStatus : MonoBehaviour
{
    //public Text timeText;
    public GameObject timeText;

    private static float time;
    private static int qnsAttempted;
    private static int qnsAnsweredCorrectly;
    public GameObject saveResultsPanel;
    public Text saveStatusMsg;
    private bool paused;


    GameObject GameManager;

    private static readonly string url = "https://smart-mario-backend-1.herokuapp.com/api/results";

    public static MatchingGameStatus instance;

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
        paused = true;
    }
    /// <summary>
    /// This method is called during every frame update to keep track of the time taken in the game 
    /// It increments the time counter by 2 decimal points unless game is paused
    /// </summary>
    void Update()
    {
        if (!paused)
        {
            time += Time.deltaTime;
        }
        timeText.GetComponent<UnityEngine.UI.Text>().text = "Time : " + time.ToString("F2");
    }
    /// <summary>
    /// This method is called to toggle the paused state of the game whenever a player presses Esc
    /// </summary>
    /// <param name="p"></param>
    public void Pause(bool p)
    {
        paused = p;
    }
    /// <summary>
    /// This method is called when the Game Status is initialised by the Manager
    /// It resets certain important variables
    /// </summary>
    public void Initialize()
    {
        qnsAttempted = 0;
        qnsAnsweredCorrectly = 0;
        GameManager = GameObject.Find("GameManager");
        Debug.Log("found game manager");
    }
    /// <summary>
    /// This method is called when the player matches a pair correctly
    /// </summary>
    public void ScoreIncrease()
    {
        qnsAnsweredCorrectly += 1;
        qnsAttempted += 1;
        Debug.Log("state : " + qnsAttempted + " " + qnsAnsweredCorrectly);
    }
    /// <summary>
    /// This method is called when the player opens two cards but does not match it correctly
    /// </summary>
    public void QnsAttemptIncrease()
    {
        qnsAttempted += 1;
        Debug.Log("state : " + qnsAttempted + " " + qnsAnsweredCorrectly);
    }
    /// <summary>
    /// This method is called when the player completes all the pairs
    /// It calls another method to compute the score and store it via a post request
    /// </summary>
    public void WinLevel()
    {
        paused = true;
        ComputeResults();
    }
    /// <summary>
    /// This method is called during the winning condition of the game
    /// It assigns two scores based on time taken and accuracy, moderated by the difficulty and level of the game being played
    /// </summary>
    private void ComputeResults()
    {
        string difficulty = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        int currentLevel = PlayerPrefs.GetInt("MinigameLevel", 1);
        int timeScore = 0;
        int accScore = 0;
        float acc = (float)qnsAnsweredCorrectly / (float)qnsAttempted;

        switch (currentLevel)
        {
            case 1:
                if (difficulty == "Easy")
                {
                    if (time < 35) { timeScore = 2000; }
                    else if (time < 60) { timeScore = 1500; }
                    else { timeScore = 1000; }
                }
                else if (difficulty == "Medium")
                {
                    if (time < 45) { timeScore = 2000; }
                    else if (time < 70) { timeScore = 1500; }
                    else { timeScore = 1000; }
                }
                else if (difficulty == "Hard")
                {
                    if (time < 55) { timeScore = 2000; }
                    else if (time < 70) { timeScore = 1500; }
                    else { timeScore = 1000; }
                }
                break;
            case 2:
                if (difficulty == "Easy")
                {
                    if (time < 50) { timeScore = 2500; }
                    else if (time < 75) { timeScore = 2000; }
                    else { timeScore = 1500; }
                }
                else if (difficulty == "Medium")
                {
                    if (time < 60) { timeScore = 2500; }
                    else if (time < 85) { timeScore = 2000; }
                    else { timeScore = 1500; }
                }
                else if (difficulty == "Hard")
                {
                    if (time < 70) { timeScore = 2500; }
                    else if (time < 95) { timeScore = 2000; }
                    else { timeScore = 1500; }
                }
                break;
            case 3:
                if (difficulty == "Easy")
                {
                    if (time < 65) { timeScore = 3000; }
                    else if (time < 90) { timeScore = 2500; }
                    else { timeScore = 2000; }
                }
                else if (difficulty == "Medium")
                {
                    if (time < 75) { timeScore = 3000; }
                    else if (time < 100) { timeScore = 2500; }
                    else { timeScore = 2000; }
                }
                else if (difficulty == "Hard")
                {
                    if (time < 85) { timeScore = 3000; }
                    else if (time < 110) { timeScore = 2500; }
                    else { timeScore = 2000; }
                }
                break;
            case 4:
                if (difficulty == "Easy")
                {
                    if (time < 80) { timeScore = 3500; }
                    else if (time < 105) { timeScore = 3000; }
                    else { timeScore = 2500; }
                }
                else if (difficulty == "Medium")
                {
                    if (time < 90) { timeScore = 3500; }
                    else if (time < 115) { timeScore = 3000; }
                    else { timeScore = 2500; }
                }
                else if (difficulty == "Hard")
                {
                    if (time < 100) { timeScore = 3500; }
                    else if (time < 125) { timeScore = 3000; }
                    else { timeScore = 2500; }
                }
                break;
            case 5:
                if (difficulty == "Easy")
                {
                    if (time < 90) { timeScore = 3500; }
                    else if (time < 115) { timeScore = 3000; }
                    else { timeScore = 2500; }
                }
                else if (difficulty == "Medium")
                {
                    if (time < 100) { timeScore = 3500; }
                    else if (time < 125) { timeScore = 3000; }
                    else { timeScore = 2500; }
                }
                else if (difficulty == "Hard")
                {
                    if (time < 110) { timeScore = 3500; }
                    else if (time < 135) { timeScore = 3000; }
                    else { timeScore = 2500; }
                }
                break;
        }
        if (acc < 0.25) 
        {
            accScore = 1500;
        }
        else if (acc < 0.5)
        {
            accScore = 2000;
        }
        else if (acc < 0.75)
        {
            accScore = 2500;
        }
        else accScore = 3000;

        Debug.Log("Time Score is " + timeScore + " and acc score is " + accScore);
        GameManager.GetComponent<Game2Control>().Win(time, qnsAnsweredCorrectly, qnsAttempted, accScore, timeScore);
        SaveResults(difficulty, currentLevel, (accScore + timeScore));
       
    }

    /// <summary>
    /// This method saves the results of the game into database after the score has been computed from local variables
    /// </summary>
    /// <param name="difficulty"></param>
    /// <param name="currentLevel"></param>
    /// <param name="score"></param>
    private void SaveResults( string difficulty, int currentLevel, int score)
    {
        int worldSelected = PlayerPrefs.GetInt("World", 1);
        string minigameSelected = PlayerPrefs.GetString("Minigame Selected", "Matching Cards");
        string studentId = "1";
        int minigameId;

        if (worldSelected == 1)
        {
            if (minigameSelected.Equals("Stranded"))
                minigameId = 1;
            else
                minigameId = 2; //2 is for matching cards world 1
        }
        else
        {
            if (minigameSelected.Equals("Stranded"))
                minigameId = 3;
            else
                minigameId = 4; //4 is for matching cards world 2
        }
        time =(float) Math.Round(time, 3);
        APICall apiCall = APICall.getAPICall();
        Debug.Log(studentId + ", " + minigameId + ", " + difficulty + ", " + currentLevel + ", " + score + qnsAttempted + ", " + qnsAnsweredCorrectly + ", " +  time);
        Results result = new Results(studentId, minigameId, difficulty, currentLevel, score, qnsAttempted, qnsAnsweredCorrectly, time);
        string bodyJsonString = apiCall.saveToJSONString(result);
        StartCoroutine(apiCall.ResultsPutRequest(bodyJsonString, url));
        Debug.Log("Sucess");
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
