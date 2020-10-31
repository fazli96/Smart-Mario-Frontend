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
    public AudioSource questionEncounterSound;
    public AudioSource questionCorrectSound;
    public AudioSource questionWrongSound;

    private Coroutine coroutine;
    private QuestionStatus qnStatus = QuestionStatus.CORRECT;

    public static StrandedQuestionManager instance;

    private static int difficulty = 1;
    private static int timeLimit = 20;
    private static List<Question> questionList;

    /// <summary>
    /// This is called before the first frame update.
    /// This method is to initialize the questions for the student based on the world and difficulty selected
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

        // initialize the question related UI panels to false
        questionPanel.SetActive(false);
        wrongPanel.SetActive(false);
        correctPanel.SetActive(false);
        tooLatePanel.SetActive(false);

        // clear the question list, useful for restart level
        questionList = new List<Question>();
        
        string difficultyStr = PlayerPrefs.GetString("Minigame Difficulty", "Easy");

        // initialize question list based on world and difficulty
        switch (difficultyStr)
        {
            case "Easy":
                //difficulty = 1;
                if (PlayerPrefs.GetInt("World", 1) == 1)
                    questionList = QuestionList.GetMcqTheoryQuestionListEasy();
                else
                    questionList = QuestionList.GetMcqCodeQuestionListEasy();
                timeLimit = 60;
                break;
            case "Medium":
                //difficulty = 2;
                if (PlayerPrefs.GetInt("World", 1) == 1)
                    questionList = QuestionList.GetMcqTheoryQuestionListMedium();
                else
                    questionList = QuestionList.GetMcqCodeQuestionListMedium();
                timeLimit = 50;
                break;
            case "Hard":
                //difficulty = 3;
                if (PlayerPrefs.GetInt("World", 1) == 1)
                    questionList = QuestionList.GetMcqTheoryQuestionListHard();
                else
                    questionList = QuestionList.GetMcqCodeQuestionListHard();
                timeLimit = 40;
                break;
            default:
                break;
        }
        Debug.Log("No of Questions: " + questionList.Count);

        /*APICall apiCall = APICall.getAPICall();
        if (PlayerPrefs.GetInt("World", 1) == 1)
            StartCoroutine(apiCall.AllQuestionsGetRequest(theoryUrl));
        else
            StartCoroutine(apiCall.AllQuestionsGetRequest(codeUrl));*/
    }


    /// <summary>
    /// This is called when the data has been retrieved from the database
    /// Questions retrieved from the database is stored as a list in this class 
    /// </summary>
    /// <param name="result"></param>
    /*public void QuestionsRetrieved(string result)
    {
        var data = (JObject)JsonConvert.DeserializeObject(result);
        JArray data2 = data["allQuestions"].Value<JArray>();
        //int counter = 0;
        foreach (JObject questionObject in data2)
        {
            Question question = questionObject.ToObject<Question>();
            if (question.difficulty == difficulty)
                questionList.Add(question);
        }
        Debug.Log("DBResult: " + result);
        Debug.Log("allQuestions" + data2);
        ShuffleList.Shuffle(questionList);
    }*/

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
        Debug.Log("answer: " + questionList[0].answer);
    }

    /// <summary>
    /// This is called when the player is prompted a question
    /// A timer is set as a countdown to the question
    /// </summary>
    public void AskQuestion()
    {
        questionEncounterSound.Play();
        SetQuestion();
        coroutine = StartCoroutine(Countdown());
        Debug.Log("After countdown");
    }

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used to count down the timer for a question
    /// </summary>
    /// <returns>Wait for Seconds</returns>
    private IEnumerator Countdown()
    {
        // if game session is multiplayer, alert other players that you are answering a question
        if (GameObject.Find("StrandedMultiplayerGameManager") != null)
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
        // countdown timer ends
        EndResult(QuestionStatus.TOOLATE);

    }

    /// <summary>
    /// This is called when the player has answered a question or
    /// the player ran out of time
    /// </summary>
    /// <param name="qnStatus"></param>
    public void EndResult(QuestionStatus _qnStatus)
    {
        // remove a question from the question list to avoid students from answering the same question
        questionList.RemoveAt(0);
        qnStatus = _qnStatus;

        // to check whether the current game session is singleplayer or multiplayer
        if (GameObject.Find("StrandedMultiplayerGameManager") != null)
        {
            if (qnStatus == QuestionStatus.CORRECT)
            {
                questionCorrectSound.Play();
                StrandedMultiplayerGameStatus.instance.ScoreChange(500);
            }  
            else
            {
                questionWrongSound.Play();
                StrandedMultiplayerGameStatus.instance.ScoreChange(-500);
            }   
        }
        else
        {
            if (qnStatus == QuestionStatus.CORRECT) 
            {
                questionCorrectSound.Play();
                StrandedGameStatus.instance.ScoreChange(500); 
            }
            else
            {
                questionWrongSound.Play();
                StrandedGameStatus.instance.ScoreChange(-500);
            }  
        }

        // stop the countdown timer coroutine
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
        // to check whether the game session is multiplayer or singleplayer
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
        if (questionList[0].answer.Equals("1"))
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
        if (questionList[0].answer.Equals("2"))
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
        if (questionList[0].answer.Equals("3"))
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
        if (questionList[0].answer.Equals("4"))
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
