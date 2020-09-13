using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    private static GameObject whoWinsTextShadow, player;

    public static int diceSideThrown = 0;
    public static int playerStartWaypoint = 0;

    public static bool gameOver = false;

    // Use this for initialization
    void Start () {

        diceSideThrown = 0;
        playerStartWaypoint = 0;
        gameOver = false;

        whoWinsTextShadow = GameObject.Find("WhoWinsText");
        whoWinsTextShadow.gameObject.SetActive(false);

        player = GameObject.Find("Player");
        player.GetComponent<FollowThePath>().moveAllowed = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver){
            //back to menu
        }

// ** PLAYER 1
        if (player.GetComponent<FollowThePath>().waypointIndex > 
            playerStartWaypoint + diceSideThrown)
        {
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
            whoWinsTextShadow.gameObject.SetActive(true);
            whoWinsTextShadow.GetComponent<Text>().text = "Player 1 Wins";
            gameOver = true;
        }
    }

    public static void MovePlayer()
    {
        player.GetComponent<FollowThePath>().moveAllowed = true;
    }
}
