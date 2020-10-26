using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System;

public class MatchingMultiplayerGameStatus : MonoBehaviour
{
    //public Text timeText;
    public GameObject timeText;

    private static float time;
    private static int qnsAttempted;
    private static int qnsAnsweredCorrectly;
    private bool start;


    GameObject GameManager;

    private static readonly string url = "https://smart-mario-backend-1.herokuapp.com/api/results";

    public static MatchingMultiplayerGameStatus instance;

    // Start is called before the first frame update
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
    void Update()
    {
        if (start)
        {
            time += Time.deltaTime;
        }
        timeText.GetComponent<UnityEngine.UI.Text>().text = "Time : " + time.ToString("F2");
    }
    public void StartGame()
    {
        start = true;
    }

    public void Initialize()
    {
        qnsAttempted = 0;
        qnsAnsweredCorrectly = 0;
        //GameManager = GameObject.Find("GameManager");
        Debug.Log("found game manager");
    }
    public void ScoreIncrease()
    {
        qnsAnsweredCorrectly += 1;
        qnsAttempted += 1;
        Debug.Log("state : " + qnsAttempted + " " + qnsAnsweredCorrectly);
    }
    public void QnsAttemptIncrease()
    {
        qnsAttempted += 1;
        Debug.Log("state : " + qnsAttempted + " " + qnsAnsweredCorrectly);
    }
    public void WinLevel()
    {
        start = false;
        ComputeResults();
    }
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
                    if (time < 1120) { timeScore = 3500; }
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
        //GameManager.GetComponent<Game2Control>().Win(time, qnsAnsweredCorrectly, qnsAttempted, accScore, timeScore);
        MatchingMultiplayerGameManager.instance.Win(time, qnsAnsweredCorrectly, qnsAttempted, accScore, timeScore);
        SaveResults(difficulty, currentLevel, (accScore + timeScore));

    }

    //code to put results into DB...
    private void SaveResults(string difficulty, int currentLevel, int score)
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
        StartCoroutine(apiCall.ResultsPutRequest(bodyJsonString, url));
        Debug.Log("Sucess");

        //APICall apiCall = APICall.getAPICall();
        //Debug.Log(studentId + ", " + minigameId + ", " + difficulty + ", " + currentLevel + ", " + currentScore + ", " + qnsAttempted + ", " + qnsAnsweredCorrectly);
        //Results result = new Results(studentId, minigameId, difficulty, currentLevel, currentScore, qnsAttempted, qnsAnsweredCorrectly);
        //string bodyJsonString = apiCall.saveToJSONString(result);
        //StartCoroutine(apiCall.ResultsPutRequest(bodyJsonString, url));
    }
}
