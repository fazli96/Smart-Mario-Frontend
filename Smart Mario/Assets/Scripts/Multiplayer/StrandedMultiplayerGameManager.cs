using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.UI;
using System;
using System.Collections;

/// <summary>
/// This class is the main controller for the Minigame Stranded in Multiplayer.
/// It implement the rules for the Minigame Stranded
/// </summary>
public class StrandedMultiplayerGameManager : MonoBehaviour
{

    public static StrandedMultiplayerGameManager instance;
    private static GameObject player;
    private static List<GameObject> questionBarrels = new List<GameObject>();
    private static List<int> mandatoryQuestionList = new List<int>();
    public List<GameObject> waypoints;

    public GameObject dice;
    public GameObject resultsPanel, surpriseQuestionPanel;
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
    /// This method is for initialization of waypoints, which is the route of player on the map, the question Barrels found on the map (only for owner),
    /// the questions to be tested to the student
    /// This method is also for spawning the player and question Barrels on the map.
    /// Player username will be attached to the player Object
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
        // clear questionBarrels, useful for restart level
        questionBarrels.Clear();

        // the last number 999 is to ensure that the mandatory question list is never null
        mandatoryQuestionList = new List<int>() { 30, 60, 90, 999 };

        resultsPanel.SetActive(false);
        surpriseQuestionPanel.SetActive(false);

        // spawn player and attached player name to gameObject
        // initialize networking variables in player gameObject
        player = SpawnPlayer(PlayerPrefs.GetString("customChar", "1"));
        player.GetComponent<PlayerPathMovement>().moveAllowed = false;
        player.GetComponent<PlayerPathMovement>().isLocalPlayer = true;
        player.GetComponent<PlayerPathMovement>().multiplayer = true;
        Transform t = player.transform.Find("Player Name Canvas");
        Transform t1 = t.transform.Find("Player Name");
        Text playerName = t1.GetComponent<Text>();
        playerName.text = NetworkManager.playerName;
        player.name = NetworkManager.playerName;

        // if player is a local player and is owner of the challenge, initialize question Barrels
        // show dice to owner as owner starts first. Otherwise, hide the dice from player
        if (NetworkManager.isOwner)
        {
            player.GetComponent<PlayerPathMovement>().isOwner = true;
            SpawnQuestionBarrels(PlayerPrefs.GetInt("MinigameLevel", 1));
            dice.SetActive(true);
        }
        else
            dice.SetActive(false);

    }

    /// <summary>
    /// This method is called for every frame. Once the player has reached the destination based on dice number rolled,
    /// check whether the tile the player lands on has a question Barrel or is the ending tile
    /// If questionBarrel, a question will pop out. If ending tile, the results and completion panel will be displayed for all players
    /// </summary>
    void LateUpdate()
    {
        // when player has reached the destination based on dice number rolled
        if (player.GetComponent<PlayerPathMovement>().waypointIndex >
            playerStartWaypoint + diceSideThrown && !qnEncountered)
        {
            // if player lands on a tile greater than the tile listed in the mandatoryQuestionList, 
            // alert player of a surprise question and show question to player
            if ((playerStartWaypoint + diceSideThrown) >= mandatoryQuestionList[0])
            {
                mandatoryQuestionList.RemoveAt(0);
                qnEncountered = true;
                leaveGameButton.SetActive(false);
                player.GetComponent<PlayerPathMovement>().moveAllowed = false;
                StartCoroutine(SurpriseQuestion());
            }

            foreach (GameObject questionBarrel in questionBarrels)
            {
                if (questionBarrel.transform.position == waypoints[playerStartWaypoint + diceSideThrown].transform.position)
                {
                    questionBarrels.Remove(questionBarrel);
                    // if mandatory/surprise question is displayed to player, do not ask question to player
                    // happens if player lands on a barrel and lands on a tile greater than the tile listed in the mandatoryQuestionList
                    if (!qnEncountered)
                    {
                        qnEncountered = true;
                        leaveGameButton.SetActive(false);
                        player.GetComponent<PlayerPathMovement>().moveAllowed = false;
                        Debug.Log("qnEncountered");
                        StrandedQuestionManager.instance.AskQuestion();
                    }
                    break;
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

            // if player is not answering a question, alert network manager that the player has ended his/her turn
            if (!qnEncountered)
            {
                Debug.Log("not question encountered");
                leaveGameButton.SetActive(true);
                currentTurn = false;
                dice.SetActive(false);
                playerStartWaypoint = player.GetComponent<PlayerPathMovement>().waypointIndex - 1;
                NetworkManager.instance.GetComponent<NetworkManager>().CommandEndTurn();
            }
        }

        // player has completed the level, alert network manager to broadcast to other players that the game has ended
        if (player.GetComponent<PlayerPathMovement>().waypointIndex == waypoints.Count && !levelComplete)
        {
            print("levelComplete" + waypoints.Count);
            NetworkManager.instance.GetComponent<NetworkManager>().CommandEndGame();
            GameComplete();
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
    /// This method is called when the game is completed
    /// </summary>
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
    /// This is called to spawn the question barrels on the board based on the level selected
    /// </summary>
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
            //if (!(rndInt == 39 || rndInt == 49))
            //{
                GameObject questionBarrelClone = Instantiate(questionBarrelPrefab,
                    waypoints[rndInt].transform.position,
                    Quaternion.Euler(0, 0, 0));
                questionBarrels.Add(questionBarrelClone);
                questionBarrelLocations.Add(rndInt);
            //}
            //Debug.Log(rndInt);
        }
        // alert network manager to broadcast to other players that the minigame has started
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
    /// This is called to make the dice object on screen to appear
    /// </summary>
    public void ShowDice()
    {
        dice.SetActive(true);
    }

    /// <summary>
    /// This is called to make the dice object on screen to dissapear
    /// </summary>
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
