using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrandedGameStatus : MonoBehaviour
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
    public static StrandedGameStatus instance;


    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        int level = PlayerPrefs.GetInt("MinigameLevel", 1);
        switch (level)
        {
            case 1:
                targetScore = currentScore = 1000;
                break;
            case 2:
                targetScore = currentScore = 1500;
                break;
            case 3:
                targetScore = currentScore = 2000;
                break;
            case 4:
                targetScore = currentScore = 2500;
                break;
            case 5:
                targetScore = currentScore = 3000;
                break;
            default:
                break;
        }
        qnsAttempted = 0;
        qnsAnsweredCorrectly = 0;
        DisplayScore();

    }
    

    /// <summary>
    /// This is to change the current score of the player
    /// </summary>
    /// <param name="changeInScore"></param>
    public void ScoreChange(int changeInScore)
    {
        if (changeInScore > 0)
        {
            qnsAnsweredCorrectly += 1;
            StrandedGameManager.instance.TeleportPlayer(true);
        }
        else
        {
            StrandedGameManager.instance.TeleportPlayer(false);
        }
        qnsAttempted += 1;
        if (!(currentScore == 0 && changeInScore < 0))
            currentScore += changeInScore;
        DisplayScore();
    }

    /// <summary>
    /// This is to display the score on the screen
    /// </summary>
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

    /// <summary>
    /// This is called when the player completes the level
    /// The Minigame Results panel will be displayed
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// This is to save the results into the database
    /// </summary>
    private void SaveResults()
    {
        int worldSelected = PlayerPrefs.GetInt("World", 1);
        string minigameSelected = PlayerPrefs.GetString("Minigame Selected", "Stranded");
        string difficulty = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        int currentLevel = PlayerPrefs.GetInt("MinigameLevel", 1);
        string studentId = PlayerPrefs.GetString("id", "1");
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
        StartCoroutine(apiCall.ResultsPutRequest(bodyJsonString, url));
    }
}

