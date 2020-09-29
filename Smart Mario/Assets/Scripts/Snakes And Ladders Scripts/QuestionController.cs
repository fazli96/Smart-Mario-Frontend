using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
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

    private Coroutine coroutine;
    private static GameStatus gameStatus;
    private QuestionStatus qnStatus = QuestionStatus.CORRECT;

    public GameStatus GameStatus { get { return gameStatus; } }

    private static int timeLimit = 20;
    private static List<Question> questionList;

    // Start is called before the first frame update
    void Awake()
    {
        questionPanel.SetActive(false);
        wrongPanel.SetActive(false);
        correctPanel.SetActive(false);
        tooLatePanel.SetActive(false);
        gameStatus = GameObject.Find("GameStatus").GetComponent<GameStatus>();

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
        //get data from database based on difficulty and level
        questionList = new List<Question>();
        List<string> options = new List<string>();
        options.Add("Boy");
        options.Add("Girl");
        options.Add("Man");
        options.Add("Woman");
        Question qn = new Question("Who are you", "text", options, "Man");
        questionList.Add(qn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetQuestion()
    {
        timer.text = "Time Left: "+timeLimit + " seconds";

        if (questionList[0].questionType == "text")
        {
            questionTitle.text = questionList[0].questionTitle;
        }
        else
        {
            //for image
        }
        option1.text = questionList[0].options[0];
        option2.text = questionList[0].options[1];
        option3.text = questionList[0].options[2];
        option4.text = questionList[0].options[3];
    }

    public void AskQuestion()
    {
        SetQuestion();
        coroutine = StartCoroutine(Countdown());
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
        if (option1.text == questionList[0].correctAns)
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
        if (option2.text == questionList[0].correctAns)
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
        if (option3.text == questionList[0].correctAns)
        {
            EndResult(QuestionStatus.CORRECT);
        }
        else
        {
            EndResult(QuestionStatus.WRONG);
        }
    }

    public void OnClickOption4()
    {
        if (option4.text == questionList[0].correctAns)
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
    [System.Serializable]
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
    }
}
