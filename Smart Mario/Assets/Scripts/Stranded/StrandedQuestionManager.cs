using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

/// <summary>
/// This class is used for retrieving and managing the questions given to the player
/// </summary>
public class StrandedQuestionManager : MonoBehaviour  
{
    public Text timer;
    public Text questionTitle;
    public Text option1;
    public Text option2;
    public Text option3;
    public Text option4;
    public GameObject questionPanel;
    public GameObject wrongPanel; 
    public GameObject correctPanel; 
    public GameObject tooLatePanel;
    private static readonly string theoryUrl = "https://smart-mario-backend-1.herokuapp.com/api/questions/mcqtheory";
    private static readonly string codeUrl = "https://smart-mario-backend-1.herokuapp.com/api/questions/mcqcode";

    private Coroutine coroutine;
    private QuestionStatus qnStatus = QuestionStatus.CORRECT;

    public static StrandedQuestionManager instance;

    private static int difficulty = 1;
    private static int timeLimit = 20;
    private static List<Question> questionList = new List<Question>();

    /// <summary>
    /// This is called before the first frame update
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
        questionPanel.SetActive(false);
        wrongPanel.SetActive(false);
        correctPanel.SetActive(false);
        tooLatePanel.SetActive(false);
        questionList.Clear();
        
        string difficultyStr = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        if (difficultyStr.Equals("Easy"))
            difficulty = 0;
        else if (difficultyStr.Equals("Medium"))
            difficulty = 1;
        else
            difficulty = 2;
        
        switch (difficultyStr)
        {
            case "Easy":
                timeLimit = 60;
                break;
            case "Medium":
                timeLimit = 50;
                break;
            case "Hard":
                timeLimit = 40;
                break;
            default:
                break;
        }
        Debug.LogError("something");
        APICall apiCall = APICall.getAPICall();
        if (PlayerPrefs.GetInt("World", 1) == 1)
            StartCoroutine(apiCall.AllQuestionsGetRequest(theoryUrl));
        else
            StartCoroutine(apiCall.AllQuestionsGetRequest(codeUrl));
    }


    /// <summary>
    /// This is called when the data has been retrieved from the database
    /// Questions retrieved from the database is stored as a list in this class 
    /// </summary>
    /// <param name="result"></param>
    public void QuestionsRetrieved(string result)
    {
        var data = (JObject)JsonConvert.DeserializeObject(result);
        JArray data2 = data["allQuestions"].Value<JArray>();
        //int counter = 0;
        foreach (JObject questionObject in data2)
        {
            //if (counter % 3 == difficulty)
            //{
                Question question = questionObject.ToObject<Question>();
                questionList.Add(question);
            //}
            //counter++;
        }
        Debug.Log("DBResult: " + result);
        Debug.Log("allQuestions" + data2);
        ShuffleList.Shuffle(questionList);
    }

    /// <summary>
    /// This is called to set the question to be displayed on the screen
    /// This is called only when the player is answering a question
    /// </summary>
    public void SetQuestion()
    {
        if (questionList.Count == 0)
            Debug.Log("Question Count is ZERO");
        timer.text = "Time Left: "+timeLimit + " seconds";

        questionTitle.text = questionList[0].questionTitle;
        option1.text = questionList[0].option1;
        option2.text = questionList[0].option2;
        option3.text = questionList[0].option3;
        option4.text = questionList[0].option4;
        questionList.RemoveAt(0);
    }

    /// <summary>
    /// This is called when the player is prompted a question
    /// A timer is set as a countdown to the question
    /// </summary>
    public void AskQuestion()
    {
        SetQuestion();
        coroutine = StartCoroutine(Countdown());
        Debug.Log("After countdown");
    }

    /// <summary>
    /// This is a coroutine to count down the timer for a question
    /// </summary>
    /// <returns>Wait for Seconds</returns>
    private IEnumerator Countdown()
    {
        if (GameObject.Find("NetworkManager") != null)
            NetworkManager.instance.CommandAnsweringQuestion();
        questionPanel.SetActive(true);
        int counter = timeLimit;
        while (counter > 0)
        {
            Debug.Log("Inside");
            timer.text = "Time Left: " + counter + " seconds";
            yield return new WaitForSeconds(1f);
            counter--;
        }
        EndResult(QuestionStatus.TOOLATE);

    }

    /// <summary>
    /// This is called when the player has answered a question or
    /// the player ran out of time
    /// </summary>
    /// <param name="qnStatus"></param>
    public void EndResult(QuestionStatus _qnStatus)
    {
        qnStatus = _qnStatus;

        if (GameObject.Find("StrandedMultiplayerGameManager") != null)
        {
            if (qnStatus == QuestionStatus.CORRECT)
                StrandedMultiplayerGameStatus.instance.ScoreChange(500);
            else
                StrandedMultiplayerGameStatus.instance.ScoreChange(-500);
        }
        else
        {
            if (qnStatus == QuestionStatus.CORRECT)
                StrandedGameStatus.instance.ScoreChange(500);
            else
                StrandedGameStatus.instance.ScoreChange(-500);
        }

        StopCoroutine(coroutine);
        coroutine = StartCoroutine(QnResults());
    }

    /// <summary>
    /// This is called to display the question results screen to the player for 2 seconds
    /// before the question results dissapear
    /// </summary>
    /// <returns>Wait for Seconds</returns>
    private IEnumerator QnResults()
    {
        if (qnStatus == QuestionStatus.CORRECT)
        {
            correctPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            correctPanel.SetActive(false);
        }
        else if (qnStatus == QuestionStatus.WRONG)
        {
            wrongPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            wrongPanel.SetActive(false);
        }
        else
        {
            tooLatePanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            tooLatePanel.SetActive(false);
        }
        questionPanel.SetActive(false);
        if (GameObject.Find("StrandedMultiplayerGameManager") != null)
            StrandedMultiplayerGameManager.qnEncountered = false;
        else
            StrandedGameManager.qnEncountered = false;
    }

    /// <summary>
    /// This is called when the first option in the question panel is pressed
    /// This is to check if the option chosen by the player is correct or wrong
    /// </summary>
    public void OnClickOption1()
    {
        if (questionList[0].answer == "1")
        {
            EndResult(QuestionStatus.CORRECT);
        }
        else
        {
            EndResult(QuestionStatus.WRONG);
        }
    }

    /// <summary>
    /// This is called when the second option in the question panel is pressed
    /// This is to check if the option chosen by the player is correct or wrong
    /// </summary>
    public void OnClickOption2()
    {
        if (questionList[0].answer == "2")
        {
            EndResult(QuestionStatus.CORRECT);
        }
        else
        {
            EndResult(QuestionStatus.WRONG);
        }
    }

    /// <summary>
    /// This is called when the third option in the question panel is pressed
    /// This is to check if the option chosen by the player is correct or wrong
    /// </summary>
    public void OnClickOption3()
    {
        if (questionList[0].answer == "3")
        {
            EndResult(QuestionStatus.CORRECT);
        }
        else
        {
            EndResult(QuestionStatus.WRONG);
        }
    }

    /// <summary>
    /// This is called when the fourth option in the question panel is pressed
    /// This is to check if the option chosen by the player is correct or wrong
    /// </summary>
    public void OnClickOption4()
    {
        if (questionList[0].answer == "4")
        {
            EndResult(QuestionStatus.CORRECT);
        }
        else
        {
            EndResult(QuestionStatus.WRONG);
        }
    }

    /// <summary>
    /// This is for easier indication on whether the question results is correct or wrong
    /// or that the player has ran out of time
    /// </summary>
    [System.Serializable]
    public enum QuestionStatus
    {
        CORRECT,
        WRONG,
        TOOLATE
    }
}
