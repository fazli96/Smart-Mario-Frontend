using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UI;
using System;
using System.Collections;

/// <summary>
/// This class is the main controller for the Minigame Stranded.
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
    public GameObject witchPrefab;
    public GameObject knightPrefab;
    public GameObject spawnPoint;

    public static int diceSideThrown = 0;
    public static int playerStartWaypoint = 0;
    public static bool qnEncountered = false;
    private static bool teleportationActive = false;
    public static bool levelComplete = false;

    /// <summary>
    /// This is for initialization
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

        diceSideThrown = 0;
        playerStartWaypoint = 0;
        levelComplete = false;
        questionBarrels.Clear();
        mandatoryQuestionList = new List<int>() { 30, 60, 90, 999 };

        completeLevelPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        surpriseQuestionPanel.SetActive(false);
        levelText.text = "Level " + PlayerPrefs.GetInt("MinigameLevel", 1);

        player = SpawnPlayer(PlayerPrefs.GetString("customChar", "1"));
        player.GetComponent<PlayerPathMovement>().moveAllowed = false;
        SpawnQuestionBarrels(PlayerPrefs.GetInt("MinigameLevel", 1));

    }
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void LateUpdate()
    {
        if (player.GetComponent<PlayerPathMovement>().waypointIndex > 
            playerStartWaypoint + diceSideThrown)
        {
            if ((playerStartWaypoint + diceSideThrown) >= mandatoryQuestionList[0] && !(playerStartWaypoint + diceSideThrown == 39 || playerStartWaypoint + diceSideThrown == 49))
            {
                mandatoryQuestionList.RemoveAt(0);
                qnEncountered = true;
                StartCoroutine(SurpriseQuestion());
            }

            foreach (GameObject questionBarrel in questionBarrels)
            {
                if (questionBarrel.transform.position == waypoints[playerStartWaypoint + diceSideThrown].transform.position)
                {
                    if (!qnEncountered)
                    {
                        qnEncountered = true;
                        StrandedQuestionManager.instance.AskQuestion();
                    }    
                    questionBarrels.Remove(questionBarrel);
                    questionBarrel.SetActive(false);
                    break;
                }
            }
            
            Debug.Log(playerStartWaypoint+diceSideThrown);
            //Teleport to another waypoint
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
        


        if (player.GetComponent<PlayerPathMovement>().waypointIndex == waypoints.Count && !levelComplete)
        {
            levelComplete = true;
            player.GetComponent<PlayerPathMovement>().moveAllowed = false;
            if (StrandedGameStatus.instance.WinLevel())
                completeLevelPanel.gameObject.SetActive(true);
            else
                gameOverPanel.gameObject.SetActive(true);
        }
    }

    IEnumerator SurpriseQuestion()
    {
        surpriseQuestionPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        surpriseQuestionPanel.SetActive(false);
        StrandedQuestionManager.instance.AskQuestion();
    }

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
    /// This is called to spawn the question barrels on the board based on the difficulty
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
    private GameObject SpawnPlayer(string selectedPlayer)
    {
        switch (selectedPlayer)
        {
            case "1":
                return Instantiate(witchPrefab,
                spawnPoint.transform.position,
                Quaternion.Euler(0, 0, 0)) as GameObject;
                //break;
            case "2":
                return Instantiate(knightPrefab,
                spawnPoint.transform.position,
                Quaternion.Euler(0, 0, 0)) as GameObject;
                //break;
            default:
                return null;
                //break;
        }
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
