using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UI;
using System;
using System.Collections;

/// <summary>
/// This class is the main controller for the Minigame Stranded in Single Player.
/// It implement the rules for the Minigame Stranded
/// </summary>
public class StrandedGameManager : MonoBehaviour {

    public static StrandedGameManager instance;
    private static GameObject player;
    private static List<GameObject> questionBarrels = new List<GameObject>();
    private static List<int> mandatoryQuestionList = new List<int>();
    public List<GameObject> waypoints;

    public GameObject dice;
    public Text levelText;
    public GameObject completeLevelPanel, gameOverPanel, surpriseQuestionPanel;
    public GameObject questionBarrelPrefab;
    public List<GameObject> characterMinigamePrefabs = new List<GameObject>();
    public GameObject spawnPoint;

    public static int diceSideThrown = 0;
    public static int playerStartWaypoint = 0;
    public static bool qnEncountered = false;
    private static bool teleportationActive = false;
    public static bool levelComplete = false;
    private static string username;

    /// <summary>
    /// This method is for initialization of waypoints, which is the route of player on the map, the question Barrels found on the map,
    /// the questions to be tested to the student
    /// This method is also for spawning the player and question Barrels on the map.
    /// Player username will be attached to the player Object
    /// </summary>
    void Start () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // initialize static variables
        diceSideThrown = 0;
        playerStartWaypoint = 0;
        qnEncountered = false;
        teleportationActive = false;
        levelComplete = false;
        username = PlayerPrefs.GetString("username", "1");

        // clear questionBarrels, useful for restart level
        questionBarrels.Clear();

        // the last number 999 is to ensure that the mandatory question list is never null
        mandatoryQuestionList = new List<int>() {20, 60, 90, 999};

        completeLevelPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        surpriseQuestionPanel.SetActive(false);
        levelText.text = "Level " + PlayerPrefs.GetInt("MinigameLevel", 1);

        // spawn player and attached player name to gameObject
        player = SpawnPlayer(int.Parse(PlayerPrefs.GetString("customChar", "0")));
        player.GetComponent<PlayerPathMovement>().moveAllowed = false;
        Transform playerTransform = player.transform.Find("Player Name Canvas");
        Transform playerTransform1 = playerTransform.transform.Find("Player Name");
        Text playerName = playerTransform1.GetComponent<Text>();
        playerName.text = username;

        SpawnQuestionBarrels(PlayerPrefs.GetInt("MinigameLevel", 1));

    }
    /// <summary>
    /// This method is called for every frame. Once the player has reached the destination based on dice number rolled,
    /// check whether the tile the player lands on has a question Barrel or is a teleportation tile or ending tile
    /// If questionBarrel or teleportation tile, a question will pop out. If ending tile, the results and completion panel will be displayed
    /// </summary>
    void LateUpdate()
    {
        // when player has reached the destination based on dice number rolled
        if (player.GetComponent<PlayerPathMovement>().waypointIndex > 
            playerStartWaypoint + diceSideThrown && !levelComplete && !qnEncountered)
        {
            // if player lands on a tile greater than the tile listed in the mandatoryQuestionList and not a teleportation tile, 
            // alert player of a surprise question and show question to player
            if ((playerStartWaypoint + diceSideThrown) >= mandatoryQuestionList[0] 
                && !(playerStartWaypoint + diceSideThrown == 39 || playerStartWaypoint + diceSideThrown == 49))
            {
                mandatoryQuestionList.RemoveAt(0);
                qnEncountered = true;
                StartCoroutine(SurpriseQuestion());
            }

            foreach (GameObject questionBarrel in questionBarrels)
            {
                if (questionBarrel.transform.position == waypoints[playerStartWaypoint + diceSideThrown].transform.position)
                {
                    // if mandatory/surprise question is displayed to player, do not ask question to player
                    // happens if player lands on a barrel and lands on a tile greater than the tile listed in the mandatoryQuestionList
                    if (!qnEncountered)
                    {
                        qnEncountered = true;
                        StrandedQuestionManager.instance.AskQuestion();
                    }    
                    questionBarrels.Remove(questionBarrel);
                    // hide question barrel on the map
                    questionBarrel.SetActive(false);
                    break;
                }
            }
            
            Debug.Log(playerStartWaypoint+diceSideThrown);
            
            // Teleport to another waypoint, player lands on a teleportation tile
            if(playerStartWaypoint+diceSideThrown == 39 || playerStartWaypoint + diceSideThrown == 49)
            {
                Debug.Log("teleportationActive");
                teleportationActive = true;
                qnEncountered = true;
                StrandedQuestionManager.instance.AskQuestion();
            }
            
            player.GetComponent<PlayerPathMovement>().moveAllowed = false;            
            playerStartWaypoint = player.GetComponent<PlayerPathMovement>().waypointIndex - 1;
        }
        
        // player has completed the level
        if (player.GetComponent<PlayerPathMovement>().waypointIndex == waypoints.Count && !levelComplete)
        {
            levelComplete = true;
            player.GetComponent<PlayerPathMovement>().moveAllowed = false;
            if (StrandedGameStatus.instance.WinLevel()) { }
                //completeLevelPanel.gameObject.SetActive(true);
            else
                gameOverPanel.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Creates an IEnumerator for coroutines that is used to alert player of surprise question for 1 second before displaying the question
    /// </summary>
    /// <returns></returns>
    IEnumerator SurpriseQuestion()
    {
        surpriseQuestionPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        surpriseQuestionPanel.SetActive(false);
        StrandedQuestionManager.instance.AskQuestion();
    }

    /// <summary>
    /// Teleport the player to another teleportation located ahead of the player if the player answers the question correctly
    /// Teleport the player to another teleportation located before the player if the player answers the question wrongly
    /// </summary>
    /// <param name="correct"></param>
    public void TeleportPlayer(bool correct)
    {
        if (correct && playerStartWaypoint == 39 && teleportationActive)
        {
            Debug.Log("teleported");
            player.GetComponent<PlayerPathMovement>().transform.position = player.GetComponent<PlayerPathMovement>().waypoints[49].transform.position;
            player.GetComponent<PlayerPathMovement>().waypointIndex = 50;
            playerStartWaypoint = player.GetComponent<PlayerPathMovement>().waypointIndex - 1;
        }
        else if (!correct && playerStartWaypoint == 49 && teleportationActive) 
        {
            Debug.Log("teleported");
            player.GetComponent<PlayerPathMovement>().transform.position = player.GetComponent<PlayerPathMovement>().waypoints[39].transform.position;
            player.GetComponent<PlayerPathMovement>().waypointIndex = 40;
            playerStartWaypoint = player.GetComponent<PlayerPathMovement>().waypointIndex - 1;
        }
        teleportationActive = false;
    }

    /// <summary>
    /// This is called when player has rolled the dice
    /// It allows player to move from one tile to the other on the board
    /// </summary>
    public static void MovePlayer()
    {
        player.GetComponent<PlayerPathMovement>().moveAllowed = true;
    }

    /// <summary>
    /// This is called to check if player is allowed to move
    /// </summary>
    /// <returns></returns>
    public static bool GetMoveAllowed()
    {
        return player.GetComponent<PlayerPathMovement>().moveAllowed;
    }

    /// <summary>
    /// This is called to spawn the question barrels on the board based on the level
    /// </summary>
    /// <param name="difficulty"></param>
    private void SpawnQuestionBarrels(int level)
    {
        int spacesBetweenBarrels;
        switch (level)
        {
            case 1:
                spacesBetweenBarrels = 6;
                break;
            case 2:
                spacesBetweenBarrels = 5;
                break;
            case 3:
                spacesBetweenBarrels = 4;
                break;
            case 4:
                spacesBetweenBarrels = 3;
                break;
            case 5:
                spacesBetweenBarrels = 2;
                break;
            default:
                spacesBetweenBarrels = 4;
                break;
        }
        List<int> questionBarrelLocations = new List<int>();
        for (int i = 2; i < 94; i += spacesBetweenBarrels)
        {

            int rndInt = UnityEngine.Random.Range(i, i + spacesBetweenBarrels);
            // exclude question barrel that is located at the teleportation tile
            if (!(rndInt == 39 || rndInt == 49))
            {
                GameObject questionBarrelClone = Instantiate(questionBarrelPrefab,
                    waypoints[rndInt].transform.position,
                    Quaternion.Euler(0, 0, 0));
                questionBarrels.Add(questionBarrelClone);
                questionBarrelLocations.Add(rndInt);
            }
        }
    }

    /// <summary>
    /// This is called to spawn the player on the board based on the character selected
    /// </summary>
    /// <param name="selectedPlayer"></param>
    /// <returns></returns>
    private GameObject SpawnPlayer(int customChar)
    {
        return Instantiate(characterMinigamePrefabs[customChar],
        spawnPoint.transform.position,
        Quaternion.Euler(0, 0, 0)) as GameObject;
    }

    /// <summary>
    /// This is to retrieve the position of all tiles on the board as a list of GameObjects
    /// </summary>
    /// <returns>the position of all tiles on the board as a list of GameObjects</returns>
    public List<GameObject> GetWayPoints()
    {
        return waypoints;
    }

    /// <summary>
    /// This is to retrieve the position of the start tile on the board as a GameObject
    /// </summary>
    /// <returns>the position of start tile on the board as a GameObject</returns>
    public GameObject GetStartWayPoint()
    {
        Debug.Log(waypoints[0]);
        return waypoints[0];
    }
}
