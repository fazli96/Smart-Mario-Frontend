using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

/// <summary>
/// This class is for the movement of the player on the board for minigame Stranded.
/// The movement of the player follows the path on the board.
/// </summary>
public class PlayerPathMovement : MonoBehaviour {

    //networking
    public bool isLocalPlayer = true;
    public bool multiplayer = false;
    public bool isOwner = false;
    Vector3 oldPosition;
    Vector3 currentPosition;

    //public Transform[] waypoints;
    public List<GameObject> waypoints = new List<GameObject>();

    [SerializeField]
    private float moveSpeed = 1f;
    private float moveH, moveV;

    [HideInInspector]
    public int waypointIndex = 0;
    public bool moveAllowed = false;
    private PlayerAnimation anim;
    private int counter = 0;
    /// <summary>
    /// This is for initialization of the waypoints on the board which will be the route of player movement
    /// This is also for the initialization of player animation to the correct character animation
    /// </summary>
    private void Start () {
        // to check whether the game Session is in multiplayer or singleplayer
        if (GameObject.Find("StrandedMultiplayerGameManager") == null)
            waypoints = StrandedGameManager.instance.GetWayPoints();
        else
            waypoints = StrandedMultiplayerGameManager.instance.GetWayPoints();
        transform.position = waypoints[waypointIndex].transform.position;
        Transform p = transform.Find("Player");
        anim = p.GetComponent<PlayerAnimation>();
    }
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update () {
        // if the player gameobject is the local player, then animate the player based on where the player moves
        if (isLocalPlayer && waypointIndex < waypoints.Count)
        {
            moveH = waypoints[waypointIndex].transform.position.x - transform.position.x;
            moveV = waypoints[waypointIndex].transform.position.y - transform.position.y;

            Vector2 direction = new Vector2(moveH, moveV);

            currentPosition = transform.position;

            // whenever the player moves, alert Network Manager to send player position to other players
            if (currentPosition != oldPosition && multiplayer)
            {
                NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(transform.position);
                oldPosition = currentPosition;
            }

            // if player is allowed to move, enable player animation
            if (moveAllowed)
            {
                Move();
                anim.SetDirection(direction);
            }
            // if player is not allowed to move, enable static player animation
            else
                anim.SetDirection(new Vector2(0, 0));
        }
        // if the player gameobject is not the local player, animate the player based on previous and current position
        else
        {
            currentPosition = transform.position;

            // animate the player only when the position of the player changes
            if (currentPosition != oldPosition)
            {
                moveH = currentPosition.x - oldPosition.x;
                moveV = currentPosition.y - oldPosition.y;

                Vector2 direction = new Vector2(moveH, moveV);

                anim.SetDirection(direction);
                oldPosition = currentPosition;
                counter = 0;
            }
            // when player is not moving for 50 frames, that means the player has stopped moving
            else 
            {
                if (counter < 50)
                    counter++;
            }
            // when player is confirmed to have stop moving, enable static player animation
            if (counter >= 50)
                anim.SetDirection(new Vector2(0, 0));
        }
        
	}

    /// <summary>
    /// Player move to the next tile on the board from current position, tile by tile
    /// Player only moves when player is allowed to move which is managed by the game Manager of that minigame
    /// </summary>
    private void Move()
    {
        // if player has not reach the end of the route provided by the waypoints, move player to the next tile
        if (waypointIndex <= waypoints.Count - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);

            // when player reaches a tile on the board as its destination, change the destination to the next tile
            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }

        }
   

    }
        
}

