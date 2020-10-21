using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsManager : MonoBehaviour
{
    //Singleton
    public static StatisticsManager instance = null;

    private SceneController scene;
    private APICall apiCall;

    public Dropdown minigameDropdown;
    public Dropdown difficultyDropdown;
    public Dropdown levelDropdown;

    public Text studentNameText;
    public Text highScoreText;
    public Text QnsCorrectText;
    public Text QnsAttemptedText;
    public Text QnsAccuracyText;

    public Button backButton;

    private static List<DisplayResults> displayResultsList;

    private static string studentName;
    private static string studentId;
    private static string minigameId;
    private static string difficulty;
    private static string level;

    public static StatisticsManager GetStatisticsManager()
    {
        if (instance == null)
        {
            instance = new StatisticsManager();
        }
        return instance;
    }


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
    }
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();        
        apiCall = APICall.getAPICall();
        
        DisplayListManager displayListManager = DisplayListManager.GetDisplayListManager();
        displayResultsList = displayListManager.GetDisplayResultsList();
        SetDefaultValues();

        studentNameText.text = studentName;

        StartCoroutine(apiCall.BestResultsGetRequest(studentId, minigameId, difficulty, level));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStudentAttributes(string newStudentName, string newStudentId)
    {
        studentId = newStudentId;
        studentName = newStudentName;
    }
    
    private void SetDefaultValues()
    {
        minigameId = "1";
        difficulty = "easy";
        level = "1"; 
    }

    public void LoadingResults()
    {
        highScoreText.text = "Loading Results...";
        QnsCorrectText.text = "";
        QnsAttemptedText.text = "";
        QnsAccuracyText.text = "";
    }

    public void ResultsAbsent()
    {
        highScoreText.text = "Level not cleared";
        QnsCorrectText.text = "";
        QnsAttemptedText.text = "";
        QnsAccuracyText.text = "";
    }

    public void ResultsRetrieved(string responseString)
    {
        var jo = JObject.Parse(responseString);
        var bodyJSONString = jo["data"].ToString();
        if (bodyJSONString.Equals("[]"))
        {
            ResultsAbsent();
        }
        else
        {
            JArray resultsListJArray = JArray.Parse(bodyJSONString);
            foreach (JObject resultJObject in resultsListJArray)
            {
                string resultJSON = JsonConvert.SerializeObject(resultJObject);
                Results result = Results.CreateFromJSON(resultJSON);
                highScoreText.text = "HighScore: " + result.score;
                QnsCorrectText.text = "Questions Answered Correctly: " + result.questions_correct;
                QnsAttemptedText.text = "Questions Attempted: " + result.questions_attempted;
                int qnsAccuracy = (result.questions_correct*100) / result.questions_attempted;
                QnsAccuracyText.text = "Questions Accuracy: " + qnsAccuracy + "%";
            }
        }
    }

    public void OnDropdownValueChange()
    {
        LoadingResults();

        minigameId = (minigameDropdown.value + 1).ToString();
        level = (levelDropdown.value + 1).ToString();
        switch (difficultyDropdown.value)
        {
            case 0:
                difficulty = "easy";
                break;
            case 1:
                difficulty = "medium";
                break;
            case 2:
                difficulty = "hard";
                break;
            default:
                difficulty = "easy";
                break;
        }
        StartCoroutine(apiCall.BestResultsGetRequest(studentId, minigameId, difficulty, level));
    }

    public void backToSelectStudent()
    {
        scene.ToSelectStudentPerformance();
    }
}
