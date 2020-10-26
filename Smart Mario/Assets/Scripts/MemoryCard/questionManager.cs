using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


/// <summary>
/// This class is used to manage the questions that players receive in minigame 2
/// </summary>
public class questionManager : MonoBehaviour
{
    public GameObject qnsOne;
    public GameObject qnsTwo;
    public GameObject panelOne;
    public GameObject panelTwo;
    private static readonly string theoryUrl = "https://smart-mario-backend-1.herokuapp.com/api/questions/shorttheory";
    private static readonly string codeUrl = "https://smart-mario-backend-1.herokuapp.com/api/questions/shortcode";
    //private static List<JObject> questionList = new List<JObject>();
    public static questionManager instance;

    private static List<Question> questionList;

    /// <summary>
    /// These actions are done before the first frame update
    /// API call is done once minigame is started
    /// </summary>
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
        qnsOne.SetActive(false);
        qnsTwo.SetActive(false);
        panelOne.SetActive(false);
        panelTwo.SetActive(false);
        //questionList.Clear();
        //APICall apiCall = APICall.getAPICall();
        //if (PlayerPrefs.GetInt("World", 1) == 1)
        //    StartCoroutine(apiCall.AllQuestionsGetRequest(theoryUrl));
        //else
        //    StartCoroutine(apiCall.AllQuestionsGetRequest(codeUrl));
        //clear the question list, useful for restart level

       questionList = new List<Question>();

       string difficultyStr = PlayerPrefs.GetString("Minigame Difficulty", "Easy");

        // initialize question list based on world and difficulty
        switch (difficultyStr)
        {
            case "Easy":
                //difficulty = 1;
                if (PlayerPrefs.GetInt("World", 1) == 1)
                    questionList = QuestionList.GetShortTheoryQuestionListEasy();
                else
                    questionList = QuestionList.GetMcqCodeQuestionListEasy();
                break;
            case "Medium":
                //difficulty = 2;
                if (PlayerPrefs.GetInt("World", 1) == 1)
                    questionList = QuestionList.GetShortTheoryQuestionListMedium();
                else
                    questionList = QuestionList.GetShortCodeQuestionListMedium();
                break;
            case "Hard":
                //difficulty = 3;
                if (PlayerPrefs.GetInt("World", 1) == 1)
                    questionList = QuestionList.GetShortTheoryQuestionListHard();
                else
                    questionList = QuestionList.GetShortCodeQuestionListHard();
                break;
            default:
                break;
        }
        Debug.Log("No of Questions: " + questionList.Count);

    }
    /// <summary>
    /// This is called when the questions are loaded from the database
    /// They stored in a list of JObjects
    /// </summary>
    /// <param name="result"></param>
    //public void QuestionsRetrieved(string result)
    //{
    //    Debug.Log("inside qns manager");
    //    var data = (JObject)JsonConvert.DeserializeObject(result);
    //    JArray data2 = data["Question"].Value<JArray>();
    //    Debug.Log("done!");
    //    foreach (JObject questionObject in data2)
    //    {
    //        Debug.Log("question: " + questionObject);
    //        //Question question1 = questionObject.ToObject<Question2>();
    //        //Debug.Log(question1.option1);
    //        //if (counter % 3 == difficulty)
    //        //{
    //            questionList.Add(questionObject);
    //        //}
    //        //counter++;
    //    }
    //    Debug.Log("DBResult: " + result);
    //    Debug.Log("Questions" + data2);
    //    ShuffleList.Shuffle(questionList);
 
    //}

    /// <summary>
    /// This is called when there is a request for the questions to be shown, called when player clicks on cards 
    /// </summary>
    /// <param name="choice"></param>
    /// <param name="index"></param>
    /// <param name="qOrA"></param>
    public void showQuestion(int choice, int index, int qOrA)
    {
        if (questionList.Count == 0)
            Debug.Log("Question Count is ZERO");
        if (choice == 1)
        {
            panelOne.SetActive(true);
            qnsOne.SetActive(true);
            Debug.Log(index.ToString());
            Debug.Log(qOrA.ToString());
            if (qOrA == 1)
            {
                qnsOne.GetComponent<UnityEngine.UI.Text>().text = questionList[index].questionTitle.ToString();
                if (PlayerPrefs.GetInt("MinigameLevel") != 5)
                {
                    qnsOne.GetComponent<UnityEngine.UI.Text>().color = new Color(126f/255f, 25f/255f, 27f/255f);
                }
            }
            else
            {
                qnsOne.GetComponent<UnityEngine.UI.Text>().text = questionList[index].answer.ToString();
                if (PlayerPrefs.GetInt("MinigameLevel") != 5)
                {
                    qnsOne.GetComponent<UnityEngine.UI.Text>().color = new Color(14f / 255f, 77f / 255f, 146f / 255f);
                }
            }
        }
        else if (choice == 2)
        {
            qnsTwo.SetActive(true);
            panelTwo.SetActive(true);
            Debug.Log("Face index is " + index.ToString());
            Debug.Log("quesiton is " + questionList[index]);
            if (qOrA == 1)
            {
                qnsTwo.GetComponent<UnityEngine.UI.Text>().text = questionList[index].questionTitle.ToString();
                if (PlayerPrefs.GetInt("MinigameLevel") != 5)
                {
                    qnsTwo.GetComponent<UnityEngine.UI.Text>().color = new Color(126f / 255f, 25f / 255f, 27f / 255f);
                }
            }
            else
            {
                qnsTwo.GetComponent<UnityEngine.UI.Text>().text = questionList[index].answer.ToString();
                if (PlayerPrefs.GetInt("MinigameLevel") != 5)
                {
                    qnsTwo.GetComponent<UnityEngine.UI.Text>().color = new Color(14f / 255f, 77f / 255f, 146f / 255f);
                }
            }
        }
        

    }
    /// <summary>
    /// This is called when a request is sent to hide the question cards
    /// A delay is induced before hiding the card
    /// </summary>
    /// <param name="choice"></param>
    /// <param name="fade"></param>
    public void hideQuestion(int choice, bool fade)
    {

        if (fade)
        {

            //StartCoroutine(WaitForSecond(1));
        }
        if (choice == 1)
        {
            qnsOne.SetActive(false);
            panelOne.SetActive(false);
        }
        else if (choice == 2)
        {
            qnsTwo.SetActive(false);
            panelTwo.SetActive(false);
        }
    }
    ///// <summary>
    ///// This delays the running of the code for a set amount of time
    ///// </summary>
    ///// <param name="time"></param>
    ///// <returns>Wait for seconds</returns>
    //IEnumerator WaitForSecond(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //}
    
}
