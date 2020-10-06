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
public class GameControl : MonoBehaviour {

    private static GameObject player;
    private static List<GameObject> questionBarrels = new List<GameObject>();
    private static List<GameObject> waypoints = new List<GameObject>();

    public GameObject dice;
    public GameObject completeLevelPanel, gameOverPanel;
    public GameObject questionBarrelPrefab;
    public GameObject witchPrefab;
    public GameObject knightPrefab;
    public GameObject spawnPoint;

    private static QuestionController questionController;
    private static GameStatus gameStatus;

    public static int diceSideThrown = 0;
    public static int playerStartWaypoint = 0;
    public static bool qnEncountered = false;

    public static bool levelComplete = false;

    // these variables are for networking
    public static bool multiplayer = false;
    public static bool currentTurn = true;
    /// <summary>
    /// This is for initialization
    /// </summary>
    void Awake () {

        diceSideThrown = 0;
        playerStartWaypoint = 0;
        levelComplete = false;
        questionBarrels.Clear();
        waypoints.Clear();

        completeLevelPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);

        questionController = GameObject.Find("QuestionController").GetComponent<QuestionController>();
        gameStatus = GameObject.Find("GameStatus").GetComponent<GameStatus>();

        player = SpawnPlayer(PlayerPrefs.GetString("Selected Player", "Witch"));
        player.GetComponent<FollowThePath>().moveAllowed = false;
        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("WayPoint"))
        {
            waypoints.Add(fooObj);
        }
        if (GameObject.Find("NetworkManager") != null)
        {
            multiplayer = true;
            player.GetComponent<FollowThePath>().isLocalPlayer = true;
            Transform t = player.transform.Find("Player Name Canvas");
            Transform t1 = t.transform.Find("Player Name");
            Text playerName = t1.GetComponent<Text>();
            playerName.text = NetworkManager.playerName;
            if (NetworkManager.isOwner)
            {
                player.GetComponent<FollowThePath>().isOwner = true;
                SpawnQuestionBarrels(PlayerPrefs.GetString("Minigame Difficulty", "Easy"));
                dice.SetActive(true);
            }
            else
                dice.SetActive(false);
        }
        else
        {
            SpawnQuestionBarrels(PlayerPrefs.GetString("Minigame Difficulty", "Easy"));
        }

        questionController.Initialize(PlayerPrefs.GetString("Minigame Difficulty", "Easy"));
        gameStatus.Initialize(PlayerPrefs.GetString("Minigame Difficulty", "Easy"));

    }
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if(levelComplete)
        {
            //update Database accordingly
        }

// ** PLAYER 1
        if (player.GetComponent<FollowThePath>().waypointIndex > 
            playerStartWaypoint + diceSideThrown)
        {
            foreach (GameObject questionBarrel in questionBarrels)
            {
                if (questionBarrel.transform.position == waypoints[playerStartWaypoint + diceSideThrown].transform.position)
                {
                    qnEncountered = true;
                    questionController.AskQuestion();
                    questionBarrels.Remove(questionBarrel);
                    questionBarrel.SetActive(false);
                    Debug.Log("After ask question");
                }
            }
            
            //Debug.Log(playerStartWaypoint+diceSideThrown);
            //Teleport to another waypoint
            /*if(playerStartWaypoint+diceSideThrown == 12){
                player.GetComponent<FollowThePath>().transform.position = player.GetComponent<FollowThePath>().waypoints[45].transform.position;
                player.GetComponent<FollowThePath>().waypointIndex = 45;
                player.GetComponent<FollowThePath>().waypointIndex +=1;
                MovePlayer();
            }*/

            //networking
            if (multiplayer && !qnEncountered)
            {
                currentTurn = false;
                NetworkManager.instance.GetComponent<NetworkManager>().CommandEndTurn();
                dice.SetActive(false);
            }
            //
            
            player.GetComponent<FollowThePath>().moveAllowed = false;            
            playerStartWaypoint = player.GetComponent<FollowThePath>().waypointIndex - 1;
        }
        


        if (player.GetComponent<FollowThePath>().waypointIndex == 
            player.GetComponent<FollowThePath>().waypoints.Count)
        {
            player.GetComponent<FollowThePath>().moveAllowed = false;
            if (gameStatus.WinLevel())
                completeLevelPanel.gameObject.SetActive(true);
            else
                gameOverPanel.gameObject.SetActive(true);
            levelComplete = true;
        }
    }

    /// <summary>
    /// This is called when player has rolled the dice
    /// It allows player to move from one tile to the other on the board
    /// </summary>
    public static void MovePlayer()
    {
        player.GetComponent<FollowThePath>().moveAllowed = true;
    }

    /// <summary>
    /// This is called to check if player is allowed to move
    /// </summary>
    /// <returns></returns>
    public static bool GetMoveAllowed()
    {
        return player.GetComponent<FollowThePath>().moveAllowed;
    }

    /// <summary>
    /// This is called to spawn the question barrels on the board based on the difficulty
    /// </summary>
    /// <param name="difficulty"></param>
    private void SpawnQuestionBarrels(string difficulty)
    {
        int spacesBetweenBarrels;
        switch (difficulty)
        {
            case "Easy":
                spacesBetweenBarrels = 6;
                break;
            case "Medium":
                spacesBetweenBarrels = 4;
                break;
            case "Hard":
                spacesBetweenBarrels = 2;
                break;
            default:
                spacesBetweenBarrels = 0;
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
        
        if (multiplayer)
        {
            NetworkManager.instance.GetComponent<NetworkManager>().CommandMinigameStart(questionBarrelLocations);
        }
    }

    /// <summary>
    /// This is called to spawn the question barrels at locations matching the host of the challenge.
    /// This is used for multiplayer
    /// </summary>
    /// <param name="questionBarrelLocations"></param>
    public void SpawnQuestionBarrelsMultiplayer(List<int> questionBarrelLocations)
    {
        for (int i = 0;i < questionBarrelLocations.Count - 1;i++)
        {
            GameObject questionBarrelClone = Instantiate(questionBarrelPrefab,
                waypoints[questionBarrelLocations[i]].transform.position,
                Quaternion.Euler(0, 0, 0));
            questionBarrels.Add(questionBarrelClone);
        }
    }

    /// <summary>
    /// This is called to make the dice object on screen dissapear
    /// </summary>
    public void ShowDice()
    {
        dice.SetActive(true);
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
    public static List<GameObject> GetWayPoints()
    {
        return waypoints;
    }

    /// <summary>
    /// This is to retrieve the position of the start tile on the board as a GameObject
    /// </summary>
    /// <returns>the position of start tile on the board as a GameObject</returns>
    public static GameObject GetStartWayPoint()
    {
        Debug.Log(waypoints[0]);
        return waypoints[0];
    }
}
