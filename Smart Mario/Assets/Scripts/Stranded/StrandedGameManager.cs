using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UI;
using System;

/// <summary>
/// This class is the main controller for the Minigame Stranded.
/// It implement the rules for the Minigame Stranded
/// </summary>
public class StrandedGameManager : MonoBehaviour {

    public static StrandedGameManager instance;
    private static GameObject player;
    private static List<GameObject> questionBarrels = new List<GameObject>();
    public List<GameObject> waypoints;

    public GameObject dice;
    public GameObject completeLevelPanel, gameOverPanel;
    public GameObject questionBarrelPrefab;
    public GameObject witchPrefab;
    public GameObject knightPrefab;
    public GameObject spawnPoint;

    public static int diceSideThrown = 0;
    public static int playerStartWaypoint = 0;
    public static bool qnEncountered = false;

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

        completeLevelPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);

        player = SpawnPlayer(PlayerPrefs.GetString("Selected Player", "Witch"));
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
            foreach (GameObject questionBarrel in questionBarrels)
            {
                if (questionBarrel.transform.position == waypoints[playerStartWaypoint + diceSideThrown].transform.position)
                {
                    qnEncountered = true;
                    StrandedQuestionManager.instance.AskQuestion();
                    questionBarrels.Remove(questionBarrel);
                    questionBarrel.SetActive(false);
                    Debug.Log("After ask question");
                }
            }
            
            //Debug.Log(playerStartWaypoint+diceSideThrown);
            //Teleport to another waypoint
            /*if(playerStartWaypoint+diceSideThrown == 12){
                player.GetComponent<PlayerPathMovement>().transform.position = player.GetComponent<PlayerPathMovement>().waypoints[45].transform.position;
                player.GetComponent<PlayerPathMovement>().waypointIndex = 45;
                player.GetComponent<PlayerPathMovement>().waypointIndex +=1;
                MovePlayer();
            }*/
            
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
            GameObject questionBarrelClone = Instantiate(questionBarrelPrefab,
                waypoints[rndInt].transform.position,
                Quaternion.Euler(0, 0, 0));
            questionBarrels.Add(questionBarrelClone);
            questionBarrelLocations.Add(rndInt);
            //Debug.Log(rndInt);
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
            case "Witch":
                return Instantiate(witchPrefab,
                spawnPoint.transform.position,
                Quaternion.Euler(0, 0, 0)) as GameObject;
                //break;
            case "Knight":
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
