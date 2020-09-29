using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
            SaveResultsToPlayerPrefs();
            return true;
        }
        else
        {
            gameOverScoreText.text = "Score: " + currentScore + " / " + targetScore;
            gameOverQnsAttemptedText.text = "Qns Attempted: " + qnsAttempted;
            gameOverQnsAnsweredCorrectlyText.text = "Qns Answered Correctly: " + qnsAnsweredCorrectly;
            SaveResultsToPlayerPrefs();
            return false;
        }
    }

    private void SaveResultsToPlayerPrefs()
    {
        string difficulty = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        int currentLevel = PlayerPrefs.GetInt("World1Minigame1Level", 1);
        string key = "World1Minigame1Level" + currentLevel+ difficulty;
        if (currentScore >= targetScore)
        {
            PlayerPrefs.SetInt("World1Minigame1HighestLevelCompleted"+difficulty, currentLevel);
            if (currentScore > PlayerPrefs.GetInt(key + "Highscore", 1000))
                PlayerPrefs.SetInt(key + "Highscore", currentScore);
        }
        PlayerPrefs.SetInt(key + "QnsAttempted", qnsAttempted);
        PlayerPrefs.SetInt(key + "QnsAnsweredCorrectly", qnsAnsweredCorrectly);
    }
}

