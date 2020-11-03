using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used for monitoring and managing the player's score, questions attempted and questions answered correctly
/// and updating these results in the database in Multiplayer game session
/// </summary>
public class StrandedMultiplayerGameStatus : MonoBehaviour
{
    public GameObject saveResultsPanel, completeLvlPanel;
    public GameObject retryButton, ignoreButton;
    public Text saveStatusMsg;
    public AudioSource errorSound;

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

    /// <summary>
    /// This method is called before the first frame update to initialize the players initial score based on the level selected
    /// This method is also for resetting the number of questions attempted and questions answered correctly
    /// </summary>
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

        int level = PlayerPrefs.GetInt("MinigameLevel", 1);
        switch (level)
        {
            case 1:
                currentScore1 = 1000;
                currentScore2 = 1000;
                currentScore3 = 1000;
                currentScore4 = 1000;
                break;
            case 2:
                currentScore1 = 1500;
                currentScore2 = 1500;
                currentScore3 = 1500;
                currentScore4 = 1500;
                break;
            case 3:
                currentScore1 = 2000;
                currentScore2 = 2000;
                currentScore3 = 2000;
                currentScore4 = 2000;
                break;
            case 4:
                currentScore1 = 2500;
                currentScore2 = 2500;
                currentScore3 = 2500;
                currentScore4 = 2500;
                break;
            case 5:
                currentScore1 = 3000;
                currentScore2 = 3000;
                currentScore3 = 3000;
                currentScore4 = 3000;
                break;
            default:
                break;
        }
        
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
    /// This is to change the current score of other players
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
                        if (!(currentScore2 == 0 && changeInScore < 0))
                            currentScore2 += changeInScore;
                        DisplayOtherPlayerScore(username);
                        break;
                    case 1:
                        if (changeInScore > 0)
                        {
                            qnsAnsweredCorrectly3 += 1;
                        }
                        qnsAttempted3 += 1;
                        if (!(currentScore3 == 0 && changeInScore < 0))
                            currentScore3 += changeInScore;
                        DisplayOtherPlayerScore(username);
                        break;
                    case 2:
                        if (changeInScore > 0)
                        {
                            qnsAnsweredCorrectly4 += 1;
                        }
                        qnsAttempted4 += 1;
                        if (!(currentScore4 == 0 && changeInScore < 0))
                            currentScore4 += changeInScore;
                        DisplayOtherPlayerScore(username);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// This is to change the current score of the player
    /// </summary>
    /// <param name="changeInScore"></param>
    public void ScoreChange(int changeInScore)
    {
        NetworkManager.instance.CommandQnResult(changeInScore);
        if (changeInScore > 0)
        {
            qnsAnsweredCorrectly1 += 1;
            StrandedMultiplayerGameManager.instance.TeleportPlayer(true);
        }
        else
        {
            StrandedMultiplayerGameManager.instance.TeleportPlayer(false);
        }
        qnsAttempted1 += 1;
        if (!(currentScore1 == 0 && changeInScore < 0))
            currentScore1 += changeInScore;
        DisplayScore(); 
    }

    /// <summary>
    /// This is to display the score of the player on the screen
    /// </summary>
    public void DisplayScore()
    {
        scoreText1.text = "Your Score: " + currentScore1;
    }

    /// <summary>
    /// This is to display the score of other players on the screen
    /// </summary>
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

    /// <summary>
    /// This is to add the username of other players to form a list of other players
    /// The list is used for reference to other players score on screen
    /// </summary>
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
    public void GameComplete()
    {
        ResultsScoreText1.text = "Your Score: " + currentScore1;
        QnsAttemptedText1.text = "Your Qns Attempted: " + qnsAttempted1;
        QnsCorrectText1.text = "Your Qns Answered Correctly: " + qnsAnsweredCorrectly1;

        // if player2 has not disconnected midway through the game, show results of player2
        if (ResultsScoreText2 != null)
        {
            ResultsScoreText2.text = players[0] + "'s Score: " + currentScore2;
            QnsAttemptedText2.text = players[0] + "'s Qns Attempted: " + qnsAttempted2;
            QnsCorrectText2.text = players[0] + "'s Qns Answered Correctly: " + qnsAnsweredCorrectly2;
        }

        // if player3 has not disconnected midway through the game, show results of player3
        if (ResultsScoreText3 != null)
        {
            ResultsScoreText3.text = players[0] + "'s Score: " + currentScore3;
            QnsAttemptedText3.text = players[0] + "'s Qns Attempted: " + qnsAttempted3;
            QnsCorrectText3.text = players[0] + "'s Qns Answered Correctly: " + qnsAnsweredCorrectly3;
        }

        // if player4 has not disconnected midway through the game, show results of player4
        if (ResultsScoreText4 != null)
        {
            ResultsScoreText4.text = players[0] + "'s Score: " + currentScore4;
            QnsAttemptedText4.text = players[0] + "'s Qns Attempted: " + qnsAttempted4;
            QnsCorrectText4.text = players[0] + "'s Qns Answered Correctly: " + qnsAnsweredCorrectly4;
        }
        SaveResults();
    }

    /// <summary>
    /// If other player is diconnected from the game, 
    /// destroy all text related to player's score, questions attempted and questions answered correctly
    /// </summary>
    /// <param name="username"></param>
    public void PlayerLeft(string username)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Equals(username) && !StrandedMultiplayerGameManager.levelComplete)
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
    public void SaveResults()
    {
        saveResultsPanel.SetActive(true);
        saveStatusMsg.text = "Saving results...";
        retryButton.SetActive(false);
        ignoreButton.SetActive(false);

        int minigameId = 0;
        switch (PlayerPrefs.GetString("Minigame Selected", "World 1 Stranded"))
        {
            case "World 1 Stranded":
                minigameId = 1;
                break;
            case "World 1 Matching Cards":
                minigameId = 2;
                break;
            case "World 2 Stranded":
                minigameId = 3;
                break;
            case "World 2 Matching Cards":
                minigameId = 4;
                break;
            default:
                break;
        }

        string difficulty = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        int currentLevel = PlayerPrefs.GetInt("MinigameLevel", 1);
        string studentId = PlayerPrefs.GetString("id", "1");

        if (currentScore1 == 0)
        {
            saveResultsPanel.SetActive(false);
            completeLvlPanel.SetActive(true);
        }
        else 
        {
            APICall apiCall = APICall.getAPICall();
            Debug.Log(studentId + ", " + minigameId + ", " + difficulty + ", " + currentLevel + ", " + currentScore1 + ", " + qnsAttempted1 + ", " + qnsAnsweredCorrectly1);
            Results result = new Results(studentId, minigameId, difficulty, currentLevel, currentScore1, qnsAttempted1, qnsAnsweredCorrectly1);
            //Results result = new Results("1", 1, "Easy", 1, 50, 1, 1);
            string bodyJsonString = apiCall.saveToJSONString(result);
            StartCoroutine(apiCall.ResultsPutRequest(bodyJsonString));
        }
    }

    /// <summary>
    /// This is to check whether the results is saved to database
    /// </summary>
    /// <param name="result"></param>
    public void ResultsSaved(string result)
    {
        if (result == null || result.Equals(""))
        {
            ShowResultsNotSaved();
        }
        else
        {
            var data = (JObject)JsonConvert.DeserializeObject(result);
            if (data["success"].ToString().Equals("True"))
            {
                saveResultsPanel.SetActive(false);
                completeLvlPanel.SetActive(true);
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
        errorSound.Play();
        completeLvlPanel.SetActive(false);
        saveResultsPanel.SetActive(true);
        saveStatusMsg.text = "Unable to save your results\n" +
            "Please check your connection\n" +
            "Click 'retry' to try again\n\n" +
            "Note: Clicking Ignore will not save your results";
        retryButton.SetActive(true);
        ignoreButton.SetActive(true);
    }
}

