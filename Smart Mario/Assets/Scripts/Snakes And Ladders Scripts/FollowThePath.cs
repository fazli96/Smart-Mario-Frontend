using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

/// <summary>
/// This class is for the movement of the player on the board for minigame Stranded.
/// The movement of the player follows the path on the board.
/// </summary>
public class FollowThePath : MonoBehaviour {

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
    /// <summary>
    /// This is for initialization
    /// </summary>
    private void Start () {

        if (GameObject.Find("NetworkManager") == null)
            waypoints = GameControl.instance.GetWayPoints();
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

        if (isLocalPlayer)
        {
            moveH = waypoints[waypointIndex].transform.position.x - transform.position.x;
            moveV = waypoints[waypointIndex].transform.position.y - transform.position.y;

            Vector2 direction = new Vector2(moveH, moveV);

            currentPosition = transform.position;

            if (currentPosition != oldPosition && multiplayer)
            {
                NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(transform.position);
                oldPosition = currentPosition;
            }

            if (moveAllowed)
            {
                Move();
                anim.SetDirection(direction);
            }
            else
                anim.SetDirection(new Vector2(0, 0));
        }
        else
        {
            currentPosition = transform.position;

            if (currentPosition != oldPosition)
            {

                moveH = currentPosition.x - oldPosition.x;
                Debug.Log("moveH: " + moveH + " = " + currentPosition.x + " - " + oldPosition.x);

                moveV = currentPosition.y - oldPosition.y;
                Debug.Log("moveV: " + moveV + " = " + currentPosition.y + " - " + oldPosition.y);

                Vector2 direction = new Vector2(moveH, moveV);

                anim.SetDirection(direction);
                oldPosition = currentPosition;
            }
            else
            {
                anim.SetDirection(new Vector2(0, 0));
            }
        }
        
	}

    /// <summary>
    /// Player move to the next tile on the board from current position
    /// </summary>
    private void Move()
    {
        
        if (waypointIndex <= waypoints.Count - 1)
        {
            //LadderCheck();
            transform.position = Vector2.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);

            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }

        }
   

    }
        
}

