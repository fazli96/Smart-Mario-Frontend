using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Control for Leaderboard providing scene transitions and setting the ranking of the top 10 players based on their total scores
/// </summary>
public class LeaderboardManager : MonoBehaviour {

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    private static SceneController scene;
    private static List<LeaderboardEntry> leaderboard;
    public static LeaderboardManager instance;
    private static readonly string url = "http://smart-mario-backend-1.herokuapp.com/api/results/leaderboard";

    /// <summary>
    /// Get instances of SceneController and leaderboard template objects once Leaderboard screen starts
    /// Initialize leaderboard to top 10 ranking players based on their total scores from the database
    /// </summary>
    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        entryContainer = transform.Find("leaderboardEntryContainer");
        entryTemplate = entryContainer.Find("leaderboardEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        scene = SceneController.GetSceneController();

        leaderboard = new List<LeaderboardEntry>();
        highscoreEntryTransformList = new List<Transform>();

        APICall apiCall = APICall.getAPICall();
        StartCoroutine(apiCall.LeaderboardGetRequest(url));

    }

    /// <summary>
    /// This method is called only when leaderboard entries are successfully retrieved from the database
    /// </summary>
    /// <param name="result"></param>
    public void LeaderboardRetrieved(string result)
    {
        var data = (JObject)JsonConvert.DeserializeObject(result);
        JArray leaderboardEntryArray = data["data"].Value<JArray>();
        //int counter = 0;
        foreach (JObject leaderboardEntryObject in leaderboardEntryArray)
        {
            LeaderboardEntry leaderboardEntry = leaderboardEntryObject.ToObject<LeaderboardEntry>();
            leaderboard.Add(leaderboardEntry);
            // append the leaderboard entry to the leaderboard template
            CreateHighscoreEntryTransform(leaderboardEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    /// <summary>
    /// This method is to instantiate and initialize a new row to the leaderboard table for every leaderboard entry retrieved from database
    /// </summary>
    /// <param name="leaderboardEntry"></param>
    /// <param name="container"></param>
    /// <param name="transformList"></param>
    private void CreateHighscoreEntryTransform(LeaderboardEntry leaderboardEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 31f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        // assign correct abreviations to the ranking position
        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
        default:
            rankString = rank + "TH"; break;

        case 1: rankString = "1ST"; break;
        case 2: rankString = "2ND"; break;
        case 3: rankString = "3RD"; break;
        }

        // initialize the text found in a row to the values assigned to the leaderboard entry
        entryTransform.Find("posText").GetComponent<Text>().text = rankString;
        string score = leaderboardEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score;
        string name = leaderboardEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        // Set background visible odds and evens, easier to read
        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);
        
        // Highlight First row, top ranking player
        if (rank == 1) {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        // Set trophy to top 3 players
        switch (rank) {
        default:
            entryTransform.Find("trophy").gameObject.SetActive(false);
            break;
        case 1:
            entryTransform.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("FFD200");
            break;
        case 2:
            entryTransform.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("C6C6C6");
            break;
        case 3:
            entryTransform.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("B76F56");
            break;

        }

        transformList.Add(entryTransform);
    }

    /// <summary>
    /// Changes scene to main menu
    /// </summary>
    public void ToMainMenu()
    {
        scene.ToMainMenu();
    }

}
