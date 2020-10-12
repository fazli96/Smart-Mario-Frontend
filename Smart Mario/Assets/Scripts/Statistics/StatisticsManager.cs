using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager instance;
    private static readonly string url = "https://smart-mario-backend-1.herokuapp.com/api/results";

    public Dropdown minigameDropdown;
    public Dropdown difficultyDropdown;
    public Dropdown levelDropdown;

    public Text highScoreText;
    public Text QnsCorrectText;
    public Text QnsAttemptedText;
    public Text QnsAccuracyText;

    private static string studentId = "1"; 

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
        LoadingResults();
        //LoadDummyData();
        
        string customUrl = url + "/"+studentId+"&1&easy&1";
        Debug.Log(customUrl);
        APICall apiCall = APICall.getAPICall();
        StartCoroutine(apiCall.BestResultsGetRequest(customUrl));
        
    }

    public void LoadDummyData()
    {
        StartCoroutine(LoadToDatabase());
    }
    IEnumerator LoadToDatabase()
    {
        List<Results> dummyResults = new List<Results>();
        Results result = new Results("1", 1, "Easy", 1, 50, 1, 1);
        Results result1 = new Results("1", 1, "Hard", 1, 100, 2, 1);
        Results result2 = new Results("1", 1, "Medium", 1, 50, 3, 1);
        Results result3 = new Results("1", 2, "Easy", 3, 50, 4, 1);
        Results result4 = new Results("1", 2, "Medium", 4, 50, 5, 1);
        Results result5 = new Results("1", 3, "Easy", 1, 50, 2, 2);
        Results result6 = new Results("1", 3, "Hard", 1, 500, 4, 3);
        Results result7 = new Results("1", 4, "Easy", 2, 5000, 5, 2);
        Results result8 = new Results("1", 4, "Medium", 3, 500, 6, 1);
        Results result9 = new Results("1", 4, "Hard", 4, 50, 7, 1);

        dummyResults.Add(result);
        dummyResults.Add(result1);
        dummyResults.Add(result2);
        dummyResults.Add(result3);
        dummyResults.Add(result4);
        dummyResults.Add(result5);
        dummyResults.Add(result6);
        dummyResults.Add(result7);
        dummyResults.Add(result8);
        dummyResults.Add(result9);

        for (int i = 0; i < dummyResults.Count; i++)
        {
            APICall apiCall = APICall.getAPICall();
            string bodyJsonString = apiCall.saveToJSONString(dummyResults[i]);
            StartCoroutine(apiCall.ResultsPutRequest(url, bodyJsonString));
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void ResultsRetrieved(string bodyJSONString)
    {
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

        int minigameId = minigameDropdown.value + 1;
        int level = levelDropdown.value + 1;
        string difficulty;
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
        string customUrl = url + "/" + studentId + "&" + minigameId + "&" + difficulty+ "&" + level;
        Debug.Log(customUrl);
        APICall apiCall = APICall.getAPICall();
        StartCoroutine(apiCall.BestResultsGetRequest(customUrl));
    }
}
