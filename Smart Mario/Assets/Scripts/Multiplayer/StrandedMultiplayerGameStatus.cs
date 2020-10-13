using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrandedMultiplayerGameStatus : MonoBehaviour
{
    public Text scoreText1;
    public Text scoreText2;
    public Text scoreText3;
    public Text scoreText4;

    public Text ResultsScoreText1;
    public Text QnsAttemptedText1;
    public Text QnsCorrectText1;
    
    public Text ResultsScoreText2;
    public Text QnsAttemptedText2;
    public Text QnsCorrectText2;
    
    public Text ResultsScoreText3;
    public Text QnsAttemptedText3;
    public Text QnsCorrectText3;
    
    public Text ResultsScoreText4;
    public Text QnsAttemptedText4;
    public Text QnsCorrectText4;

    
    private static int currentScore1;
    private static int qnsAttempted1;
    private static int qnsAnsweredCorrectly1;

    private static int currentScore2;
    private static int qnsAttempted2;
    private static int qnsAnsweredCorrectly2;

    private static int currentScore3;
    private static int qnsAttempted3;
    private static int qnsAnsweredCorrectly3;

    private static int currentScore4;
    private static int qnsAttempted4;
    private static int qnsAnsweredCorrectly4;

    private static List<string> players;

    public static StrandedMultiplayerGameStatus instance;
    private static readonly string url = "https://smart-mario-backend-1.herokuapp.com/api/results";


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
        players = new List<string>();

        currentScore1 = 3000;
        currentScore2 = 3000;
        currentScore3 = 3000;
        currentScore4 = 3000;
        qnsAttempted1 = 0;
        qnsAttempted2 = 0;
        qnsAttempted3 = 0;
        qnsAttempted4 = 0;
        qnsAnsweredCorrectly1 = 0;
        qnsAnsweredCorrectly2 = 0;
        qnsAnsweredCorrectly3 = 0;
        qnsAnsweredCorrectly4 = 0;
        DisplayScore();
    }

    /// <summary>
    /// This is to change the current score of the player
    /// </summary>
    /// <param name="changeInScore"></param>
    public void OtherPlayerScoreChange(string username, int changeInScore)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Equals(username))
            {
                switch (i)
                {
                    case 0:
                        if (changeInScore > 0)
                        {
                            qnsAnsweredCorrectly2 += 1;
                        }
                        qnsAttempted2 += 1;
                        currentScore2 += changeInScore;
                        DisplayOtherPlayerScore(username);
                        break;
                    case 1:
                        if (changeInScore > 0)
                        {
                            qnsAnsweredCorrectly3 += 1;
                        }
                        qnsAttempted3 += 1;
                        currentScore3 += changeInScore;
                        DisplayOtherPlayerScore(username);
                        break;
                    case 2:
                        if (changeInScore > 0)
                        {
                            qnsAnsweredCorrectly4 += 1;
                        }
                        qnsAttempted4 += 1;
                        currentScore4 += changeInScore;
                        DisplayOtherPlayerScore(username);
                        break;
                }
            }
        }
    }

    public void ScoreChange(int changeInScore)
    {
        if (changeInScore > 0)
        {
            qnsAnsweredCorrectly1 += 1;
        }
        qnsAttempted1 += 1;
        currentScore1 += changeInScore;
        DisplayScore();
        NetworkManager.instance.CommandQnResult(changeInScore);
    }

    /// <summary>
    /// This is to display the score on the screen
    /// </summary>
    public void DisplayScore()
    {
        scoreText1.text = "Your Score: " + currentScore1;
    }

    public void DisplayOtherPlayerScore(string username)
    {
        Debug.Log("Player Count: " + players.Count);
        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log(players[i]);
            if (players[i].Equals(username))
            {
                switch (i)
                {
                    case 0:
                        scoreText2.text = username + "'s Score: " + currentScore2;
                        break;
                    case 1:
                        scoreText3.text = username + "'s Score: " + currentScore3;
                        break;
                    case 2:
                        scoreText4.text = username + "'s Score: " + currentScore4;
                        break;
                }
            }
        }
    }

    public void AddPlayer(string username)
    {
        Debug.Log("Add Player " + username);
        players.Add(username);
        Debug.Log("Player " + players[0]);
        DisplayOtherPlayerScore(username);
    }

    /// <summary>
    /// This is called when the player completes the level
    /// The Minigame Results panel will be displayed
    /// </summary>
    /// <returns></returns>
    public bool GameComplete()
    {
        ResultsScoreText1.text = "Your Score: " + currentScore1;
        QnsAttemptedText1.text = "Your Qns Attempted: " + qnsAttempted1;
        QnsCorrectText1.text = "Your Qns Answered Correctly: " + qnsAnsweredCorrectly1;

        if (ResultsScoreText2 != null && players.Count > 0)
        {
            ResultsScoreText2.text = players[0] + "'s Score: " + currentScore2;
            QnsAttemptedText2.text = players[0] + "'s Qns Attempted: " + qnsAttempted2;
            QnsCorrectText2.text = players[0] + "'s Qns Answered Correctly: " + qnsAnsweredCorrectly2;
        }

        if (ResultsScoreText3 != null && players.Count > 1)
        {
            ResultsScoreText3.text = players[0] + "'s Score: " + currentScore3;
            QnsAttemptedText3.text = players[0] + "'s Qns Attempted: " + qnsAttempted3;
            QnsCorrectText3.text = players[0] + "'s Qns Answered Correctly: " + qnsAnsweredCorrectly3;
        }

        if (ResultsScoreText4 != null && players.Count > 2)
        {
            ResultsScoreText4.text = players[0] + "'s Score: " + currentScore4;
            QnsAttemptedText4.text = players[0] + "'s Qns Attempted: " + qnsAttempted4;
            QnsCorrectText4.text = players[0] + "'s Qns Answered Correctly: " + qnsAnsweredCorrectly4;
        }
        //SaveResults();
        return true;
    }

    public void PlayerLeft(string username)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Equals(username))
            {
                switch (i)
                {
                    case 0:
                        Destroy(scoreText2.gameObject);
                        Destroy(ResultsScoreText2.gameObject);
                        Destroy(QnsAttemptedText2.gameObject);
                        Destroy(QnsCorrectText2.gameObject);
                        break;
                    case 1:
                        Destroy(scoreText3.gameObject);
                        Destroy(ResultsScoreText3.gameObject);
                        Destroy(QnsAttemptedText3.gameObject);
                        Destroy(QnsCorrectText3.gameObject);
                        break;
                    case 2:
                        Destroy(scoreText4.gameObject);
                        Destroy(ResultsScoreText4.gameObject);
                        Destroy(QnsAttemptedText4.gameObject);
                        Destroy(QnsCorrectText4.gameObject);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// This is to save the results into the database
    /// </summary>
    /*private void SaveResults()
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
        Debug.Log(studentId + ", " + minigameId + ", " + difficulty + ", " + currentLevel + ", " + currentScore1 + ", " + qnsAttempted1 + ", " + qnsAnsweredCorrectly1);
        Results result = new Results(studentId, minigameId, difficulty, currentLevel, currentScore1, qnsAttempted1, qnsAnsweredCorrectly1);
        //Results result = new Results("1", 1, "Easy", 1, 50, 1, 1);
        string bodyJsonString = apiCall.saveToJSONString(result);
        StartCoroutine(apiCall.ResultsPutRequest(url, bodyJsonString));
    }*/
}

