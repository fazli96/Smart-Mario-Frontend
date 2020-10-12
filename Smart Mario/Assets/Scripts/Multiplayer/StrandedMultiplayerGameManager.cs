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
public class StrandedMultiplayerGameManager : MonoBehaviour
{

    public static StrandedMultiplayerGameManager instance;
    private static GameObject player;
    private static List<GameObject> questionBarrels = new List<GameObject>();
    public List<GameObject> waypoints;

    public GameObject dice;
    public GameObject resultsPanel;
    public GameObject leaveGameButton;
    public GameObject questionBarrelPrefab;
    public GameObject witchPrefab;
    public GameObject knightPrefab;
    public GameObject spawnPoint;

    public static int diceSideThrown = 0;
    public static int playerStartWaypoint = 0;
    public static bool qnEncountered = false;

    public static bool levelComplete = false;

    // these variables are for networking
    public static bool currentTurn = true;
    /// <summary>
    /// This is for initialization
    /// </summary>
    void Start()
    {
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

        player = SpawnPlayer(PlayerPrefs.GetString("Selected Player", "Witch"));
        player.GetComponent<PlayerPathMovement>().moveAllowed = false;
        player.GetComponent<PlayerPathMovement>().isLocalPlayer = true;
        player.GetComponent<PlayerPathMovement>().multiplayer = true;
        Transform t = player.transform.Find("Player Name Canvas");
        Transform t1 = t.transform.Find("Player Name");
        Text playerName = t1.GetComponent<Text>();
        playerName.text = NetworkManager.playerName;
        player.name = NetworkManager.playerName;
        if (NetworkManager.isOwner)
        {
            player.GetComponent<PlayerPathMovement>().isOwner = true;
            SpawnQuestionBarrels();
            dice.SetActive(true);
        }
        else
            dice.SetActive(false);
        
        StrandedQuestionManager.instance.Initialize(PlayerPrefs.GetString("Minigame Difficulty", "Easy"));
        StrandedMultiplayerGameStatus.instance.Initialize(PlayerPrefs.GetString("Minigame Difficulty", "Easy"));

    }
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void LateUpdate()
    {
       
        if (player.GetComponent<PlayerPathMovement>().waypointIndex >
            playerStartWaypoint + diceSideThrown && !qnEncountered)
        {
            foreach (GameObject questionBarrel in questionBarrels)
            {
                if (questionBarrel.transform.position == waypoints[playerStartWaypoint + diceSideThrown].transform.position)
                {
                    qnEncountered = true;
                    leaveGameButton.SetActive(false);
                    Debug.Log("qnEncountered");
                    questionBarrels.Remove(questionBarrel);
                    player.GetComponent<PlayerPathMovement>().moveAllowed = false;
                    StrandedQuestionManager.instance.AskQuestion();
                }
            }

            player.GetComponent<PlayerPathMovement>().moveAllowed = false;

            //Debug.Log(playerStartWaypoint+diceSideThrown);
            //Teleport to another waypoint
            /*if(playerStartWaypoint+diceSideThrown == 12){
                player.GetComponent<PlayerPathMovement>().transform.position = player.GetComponent<PlayerPathMovement>().waypoints[45].transform.position;
                player.GetComponent<PlayerPathMovement>().waypointIndex = 45;
                player.GetComponent<PlayerPathMovement>().waypointIndex +=1;
                MovePlayer();
            }*/

            //networking
            if (!qnEncountered)
            {
                Debug.Log("not question encountered");
                leaveGameButton.SetActive(true);
                currentTurn = false;
                dice.SetActive(false);
                playerStartWaypoint = player.GetComponent<PlayerPathMovement>().waypointIndex - 1;
                NetworkManager.instance.GetComponent<NetworkManager>().CommandEndTurn();
            }
            //

            
        }



        if (player.GetComponent<PlayerPathMovement>().waypointIndex == waypoints.Count && !levelComplete)
        {
            print("levelComplete" + waypoints.Count);
            NetworkManager.instance.GetComponent<NetworkManager>().CommandEndGame();
            GameComplete();
        }
    }

    public void GameComplete()
    {
        levelComplete = true;
        leaveGameButton.SetActive(false);
        HideDice();
        player.GetComponent<PlayerPathMovement>().moveAllowed = false;
        if (StrandedMultiplayerGameStatus.instance.GameComplete())
        {
            resultsPanel.gameObject.SetActive(true);
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
    /// This is called to spawn the question barrels on the board
    /// </summary>
    private void SpawnQuestionBarrels()
    {
        int spacesBetweenBarrels = 3;
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
        NetworkManager.instance.GetComponent<NetworkManager>().CommandMinigameStart(questionBarrelLocations);
        
    }

    /// <summary>
    /// This is called to spawn the question barrels at locations matching the host of the challenge.
    /// This is used for multiplayer
    /// </summary>
    /// <param name="questionBarrelLocations"></param>
    public void SpawnQuestionBarrelsMultiplayer(List<int> questionBarrelLocations)
    {
        for (int i = 0; i < questionBarrelLocations.Count; i++)
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

    public void HideDice()
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
