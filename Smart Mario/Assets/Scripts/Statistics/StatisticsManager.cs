using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller class for the statistics scene.
/// </summary>
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
    private static bool teacherUser;

    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Set the attributes of the student
    /// </summary>
    /// <param name="newStudentName"></param>
    /// <param name="newStudentId"></param>
    /// <param name="teacherUserBool"></param>
    public void SetStudentAttributes(string newStudentName, string newStudentId, bool teacherUserBool)
    {
        studentId = newStudentId;
        studentName = newStudentName;
        teacherUser = teacherUserBool;
    }
    
    /// <summary>
    /// Default dropdown list values
    /// </summary>
    private void SetDefaultValues()
    {
        minigameId = "1";
        difficulty = "easy";
        level = "1"; 
    }

    /// <summary>
    /// Displays loading results message when retrieving from the database
    /// </summary>
    public void LoadingResults()
    {
        highScoreText.text = "Loading Results...";
        QnsCorrectText.text = "";
        QnsAttemptedText.text = "";
        QnsAccuracyText.text = "";
    }

    /// <summary>
    /// Displays message if student did not complete the level
    /// </summary>
    public void ResultsAbsent()
    {
        highScoreText.text = "Level not cleared";
        QnsCorrectText.text = "";
        QnsAttemptedText.text = "";
        QnsAccuracyText.text = "";
    }

    /// <summary>
    /// Displays the results of the level if exists and completed
    /// </summary>
    /// <param name="responseString"></param>
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

    /// <summary>
    /// Changes current value when user clicks on one from the dropdown menu
    /// </summary>
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

    /// <summary>
    /// Back button action
    /// </summary>
    public void backToSelectStudent()
    {
        if (teacherUser)
        {
            scene.ToSelectStudentPerformance();
        }
        else
        {
            scene.ToMainMenu();
        }
    }
}
