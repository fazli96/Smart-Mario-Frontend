using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;

public class GameStatus : MonoBehaviour
{

    public Text scoreText;
    public Text completeLvlScoreText;
    public Text gameOverScoreText;
    public Text gameOverQnsAttemptedText;
    public Text gameOverQnsAnsweredCorrectlyText;
    public Text completeLvlQnsAttemptedText;
    public Text completeLvlQnsAnsweredCorrectlyText;

    private static int targetScore;
    private static int currentScore;
    private static int qnsAttempted;
    private static int qnsAnsweredCorrectly;

    private static readonly string url = "https://smart-mario-backend-1.herokuapp.com/api/results";


    public void Initialize(string difficulty)
    {
        switch(difficulty)
        {
            case "Easy":
                targetScore = currentScore = 1000;
                break;
            case "Medium":
                targetScore = currentScore = 2000;
                break;
            case "Hard":
                targetScore = currentScore = 3000;
                break;
            default:
                break;
        }
        qnsAttempted = 0;
        qnsAnsweredCorrectly = 0;
        DisplayScore();
        TestGetResults();
    }

    public void ScoreChange(int changeInScore)
    {
        if (changeInScore > 0)
        {
            qnsAnsweredCorrectly += 1;
        }
        qnsAttempted += 1;
        currentScore += changeInScore;
        DisplayScore();
    }

    public void DisplayScore()
    {
        scoreText.text = "Score: " + currentScore + " / " + targetScore;
        if (currentScore >= targetScore)
        {
            scoreText.color = Color.green;
        }
        else
        {
            scoreText.color = Color.red;
        }
    }

    public bool WinLevel()
    {
        if (currentScore >= targetScore)
        {
            completeLvlScoreText.text = "Score: " + currentScore + " / " + targetScore;
            completeLvlQnsAttemptedText.text = "Qns Attempted: " + qnsAttempted;
            completeLvlQnsAnsweredCorrectlyText.text = "Qns Answered Correctly: " + qnsAnsweredCorrectly;
            SaveResults();
            return true;
        }
        else
        {
            gameOverScoreText.text = "Score: " + currentScore + " / " + targetScore;
            gameOverQnsAttemptedText.text = "Qns Attempted: " + qnsAttempted;
            gameOverQnsAnsweredCorrectlyText.text = "Qns Answered Correctly: " + qnsAnsweredCorrectly;
            SaveResults();
            return false;
        }
    }

    private void SaveResults()
    {
        int worldSelected = PlayerPrefs.GetInt("World", 1);
        string minigameSelected = PlayerPrefs.GetString("Minigame Selected", "Stranded");
        string difficulty = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        int currentLevel = PlayerPrefs.GetInt("MinigameLevel", 1);
        string studentId = "1";
        int minigameId;

        if (worldSelected == 1)
        {
            if (minigameSelected.Equals("Stranded"))
                minigameId = 1;
            else
                minigameId = 2;
        }
        else
        {
            if (minigameSelected.Equals("Stranded"))
                minigameId = 3;
            else
                minigameId = 4;
        }

        APICall apiCall = APICall.getAPICall();
        Debug.Log(studentId + ", " + minigameId + ", " + difficulty + ", " + currentLevel + ", " + currentScore + ", " + qnsAttempted + ", " + qnsAnsweredCorrectly);
        Results result = new Results(studentId, minigameId, difficulty, currentLevel, currentScore, qnsAttempted, qnsAnsweredCorrectly);
        //Results result = new Results("1", 1, "Easy", 1, 50, 1, 1);
        string bodyJsonString = apiCall.saveToJSONString(result);
        StartCoroutine(apiCall.ResultsPutRequest(url, bodyJsonString));
    }

    public void TestGetResults()
    {
        int worldSelected = PlayerPrefs.GetInt("World", 1);
        string minigameSelected = PlayerPrefs.GetString("Minigame Selected", "Stranded");
        string difficulty = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        int currentLevel = PlayerPrefs.GetInt("MinigameLevel", 1);
        string studentId = "1";
        int minigameId;

        if (worldSelected == 1)
        {
            if (minigameSelected.Equals("Stranded"))
                minigameId = 1;
            else
                minigameId = 2;
        }
        else
        {
            if (minigameSelected.Equals("Stranded"))
                minigameId = 3;
            else
                minigameId = 4;
        }

        string customUrl = url + "/" + studentId + "&" + minigameId + "&" + difficulty + "&" + currentLevel;
        //customUrl.ToLower();
        Debug.Log(customUrl);
        APICall apiCall = APICall.getAPICall();
        StartCoroutine(apiCall.BestResultsGetRequest(url));
    }
}

