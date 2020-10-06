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

public class QuestionController : MonoBehaviour  
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
    private static readonly string url = "https://smart-mario-backend-1.herokuapp.com/api/questions/mcqtheory"; 

    private Coroutine coroutine;
    private static GameStatus gameStatus;
    private QuestionStatus qnStatus = QuestionStatus.CORRECT;

    public GameStatus GameStatus { get { return gameStatus; } }

    private static int timeLimit = 20;
    private static List<Question> questionList = new List<Question>();

    // Start is called before the first frame update
    void Awake()
    {
        questionPanel.SetActive(false);
        wrongPanel.SetActive(false);
        correctPanel.SetActive(false);
        tooLatePanel.SetActive(false);
        gameStatus = GameObject.Find("GameStatus").GetComponent<GameStatus>();
        timeLimit = 20;
        questionList.Clear();

    }

    public void Initialize(string difficulty)
    {
        switch (difficulty)
        {
            case "Easy":
                timeLimit = 25;
                break;
            case "Medium":
                timeLimit = 20;
                break;
            case "Hard":
                timeLimit = 15;
                break;
            default:
                break;
        }
        APICall apiCall = APICall.getAPICall();
        StartCoroutine(apiCall.AllQuestionsGetRequest(url));
    }

    public void QuestionsRetrieved(string result)
    {
        var data = (JObject)JsonConvert.DeserializeObject(result);
        JArray data2 = data["allQuestions"].Value<JArray>();
        foreach (JObject questionObject in data2)
        {
            Debug.Log("question: " + questionObject);
            Question question1 = questionObject.ToObject<Question>();
            Debug.Log(question1.option1);
            questionList.Add(question1);
        }
        Debug.Log("DBResult: " + result);
        Debug.Log("allQuestions" + data2);
        ShuffleList.Shuffle(questionList);
    }

    public void SetQuestion()
    {
        timer.text = "Time Left: "+timeLimit + " seconds";

        questionTitle.text = questionList[0].questionTitle;
        option1.text = questionList[0].option1;
        option2.text = questionList[0].option2;
        option3.text = questionList[0].option3;
        option4.text = questionList[0].option4;
        questionList.RemoveAt(0);
    }

    public void AskQuestion()
    {
        SetQuestion();
        coroutine = StartCoroutine(Countdown());
        Debug.Log("After countdown");
    }

    private IEnumerator Countdown()
    {
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

    public void EndResult(QuestionStatus qnStatus)
    {
        this.qnStatus = qnStatus;
        if (qnStatus == QuestionStatus.CORRECT)
            gameStatus.ScoreChange(500);
        else
            gameStatus.ScoreChange(-500);
        //questionList.RemoveAt(0);
        StopCoroutine(coroutine);
        coroutine = StartCoroutine(QnResults());
    }

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
        GameControl.qnEncountered = false;
    }

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

    public void OnClickOption3()
    {
        if (questionList[0].answer == "3")
        {
            EndResult(QuestionStatus.CORRECT);
        }
        else
        {
            Debug.Log(option3.text + " = " + questionList[0].answer);
            EndResult(QuestionStatus.WRONG);
        }
    }

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

    [System.Serializable]
    public enum QuestionStatus
    {
        CORRECT,
        WRONG,
        TOOLATE
    }

    //Datastructure for storeing the quetions data
    /*[System.Serializable]
    public class Question
    {
        public string questionTitle;         //question text
        public string questionType;          //type
        //public Sprite questionImage;        //image for Image Type
        public List<string> options;        //options to select
        public string correctAns;           //correct option

        public Question(string questionTitle, string questionType, List<string> options, string correctAns)
        {
            this.questionTitle = questionTitle;
            this.questionType = questionType;
            this.options = options;
            this.correctAns = correctAns;
        }
    }*/

    /*public IEnumerator Get(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    // handle the result
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    var data = (JObject)JsonConvert.DeserializeObject(result);
                    JArray data2 = data["allQuestions"].Value<JArray>();
                    foreach (JObject questionObject in data2)
                    {
                        Debug.Log("question: " + questionObject);
                        Question question1 = questionObject.ToObject<Question>();
                        Debug.Log(question1.option1);
                        questionList.Add(question1);  
                    }
                    Debug.Log("DBResult: "+result);
                    Debug.Log("allQuestions" + data2);
                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }

    }*/
}
