using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

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

    // Use this for initialization
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

        SpawnQuestionBarrels(PlayerPrefs.GetString("Minigame Difficulty","Easy"));

        questionController.Initialize(PlayerPrefs.GetString("Minigame Difficulty", "Easy"));
        gameStatus.Initialize(PlayerPrefs.GetString("Minigame Difficulty", "Easy"));

    }

    // Update is called once per frame
    void Update()
    {
        if(levelComplete)
        {
            //update Database accordingly
        }

        if (player.GetComponent<FollowThePath>().moveAllowed || qnEncountered)
        {
            dice.SetActive(false);
        }
        else
        {
            dice.SetActive(true);
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

    public static void MovePlayer()
    {
        player.GetComponent<FollowThePath>().moveAllowed = true;
    }

    public static bool getMoveAllowed()
    {
        return player.GetComponent<FollowThePath>().moveAllowed;
    }

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
        for (int i = 2; i < 94; i += spacesBetweenBarrels)
        {

            int rndInt = Random.Range(i, i + spacesBetweenBarrels);
            GameObject questionBarrelClone = Instantiate(questionBarrelPrefab,
                waypoints[rndInt].transform.position,
                Quaternion.Euler(0, 0, 0));
            questionBarrels.Add(questionBarrelClone);
            //Debug.Log(rndInt);
        }
    }

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

    public static List<GameObject> GetWayPoints()
    {
        return waypoints;
    }
}
