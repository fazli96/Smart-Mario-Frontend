using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

public class GameControl : MonoBehaviour {

    private static GameObject player;
    private static List<int> questionBarrelLocation = new List<int>();
    private static List<GameObject> waypoints = new List<GameObject>();
    public GameObject completeLevelPanel, gameOverPanel;
    public GameObject questionBarrelPrefab;
    public GameObject questionBarrelClone;
    public GameObject witchPrefab;
    public GameObject knightPrefab;
    public GameObject spawnPoint;

    public static int diceSideThrown = 0;
    public static int playerStartWaypoint = 0;

    public static bool levelComplete = false;

    // Use this for initialization
    void Start () {

        diceSideThrown = 0;
        playerStartWaypoint = 0;
        levelComplete = false;
        questionBarrelLocation.Clear();

        completeLevelPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);

        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("WayPoint"))
        {
            waypoints.Add(fooObj);
        }

        player = SpawnPlayer(PlayerPrefs.GetString("Selected Player", "Witch"));
        player.GetComponent<FollowThePath>().moveAllowed = false;

        SpawnQuestionBarrels(PlayerPrefs.GetInt("Minigame Difficulty",3));

    }

    // Update is called once per frame
    void Update()
    {
        if(levelComplete)
        {
            //back to menu
        }

// ** PLAYER 1
        if (player.GetComponent<FollowThePath>().waypointIndex > 
            playerStartWaypoint + diceSideThrown)
        {
            if (questionBarrelLocation.Contains(playerStartWaypoint + diceSideThrown))
            {
                Debug.Log("ask a question");
            }
            
            //Debug.Log(playerStartWaypoint+diceSideThrown);
            /*if(playerStartWaypoint+diceSideThrown == 12){
                player.GetComponent<FollowThePath>().transform.position = player.GetComponent<FollowThePath>().waypoints[45].transform.position;
                player.GetComponent<FollowThePath>().waypointIndex = 45;
                player.GetComponent<FollowThePath>().waypointIndex +=1;
                MovePlayer();
            }
            if(playerStartWaypoint+diceSideThrown == 32){
                player.GetComponent<FollowThePath>().transform.position = player.GetComponent<FollowThePath>().waypoints[48].transform.position;
                player.GetComponent<FollowThePath>().waypointIndex = 48;
                player.GetComponent<FollowThePath>().waypointIndex +=1;
                MovePlayer();
            }
            if(playerStartWaypoint+diceSideThrown == 41){
                player.GetComponent<FollowThePath>().transform.position = player.GetComponent<FollowThePath>().waypoints[62].transform.position;
                player.GetComponent<FollowThePath>().waypointIndex = 62;
                player.GetComponent<FollowThePath>().waypointIndex +=1;
                MovePlayer();
            }
            if(playerStartWaypoint+diceSideThrown == 49){
                player.GetComponent<FollowThePath>().transform.position = player.GetComponent<FollowThePath>().waypoints[68].transform.position;
                player.GetComponent<FollowThePath>().waypointIndex = 68;
                player.GetComponent<FollowThePath>().waypointIndex +=1;
                MovePlayer();
            }
            if(playerStartWaypoint+diceSideThrown == 61){
                player.GetComponent<FollowThePath>().transform.position = player.GetComponent<FollowThePath>().waypoints[80].transform.position;
                player.GetComponent<FollowThePath>().waypointIndex = 80;
                player.GetComponent<FollowThePath>().waypointIndex +=1;
                MovePlayer();
            }
            if(playerStartWaypoint+diceSideThrown == 73){
                player.GetComponent<FollowThePath>().transform.position = player.GetComponent<FollowThePath>().waypoints[91].transform.position;
                player.GetComponent<FollowThePath>().waypointIndex = 91;
                player.GetComponent<FollowThePath>().waypointIndex +=1;
                MovePlayer();
            }
            if(playerStartWaypoint+diceSideThrown == 39){
                player.GetComponent<FollowThePath>().transform.position = player.GetComponent<FollowThePath>().waypoints[2].transform.position;
                player.GetComponent<FollowThePath>().waypointIndex = 2;
                player.GetComponent<FollowThePath>().waypointIndex +=1;
                MovePlayer();
            }
            if(playerStartWaypoint+diceSideThrown == 86){
                player.GetComponent<FollowThePath>().transform.position = player.GetComponent<FollowThePath>().waypoints[36].transform.position;
                player.GetComponent<FollowThePath>().waypointIndex = 36;
                player.GetComponent<FollowThePath>().waypointIndex +=1;
                MovePlayer();
            }
            if(playerStartWaypoint+diceSideThrown == 88){
                player.GetComponent<FollowThePath>().transform.position = player.GetComponent<FollowThePath>().waypoints[52].transform.position;
                player.GetComponent<FollowThePath>().waypointIndex = 52;
                player.GetComponent<FollowThePath>().waypointIndex +=1;
                MovePlayer();
            }
            if(playerStartWaypoint+diceSideThrown == 97){
                player.GetComponent<FollowThePath>().transform.position = player.GetComponent<FollowThePath>().waypoints[40].transform.position;
                player.GetComponent<FollowThePath>().waypointIndex = 40;
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
            completeLevelPanel.gameObject.SetActive(true);
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

    private void SpawnQuestionBarrels(int difficulty)
    {
        // difficulty Easy - 3, Medium - 2, Hard - 1
        for (int i = 2; i < 94; i += (difficulty * 2))
        {

            int rndInt = Random.Range(i, i + 5);
            questionBarrelLocation.Add(rndInt);
            //Debug.Log(rndInt);
        }

        for (int i = 0; i < questionBarrelLocation.Count() - 1; i++)
        {
            questionBarrelClone = Instantiate(questionBarrelPrefab,
                waypoints[questionBarrelLocation[i]].transform.position,
                Quaternion.Euler(0, 0, 0)) as GameObject;
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
