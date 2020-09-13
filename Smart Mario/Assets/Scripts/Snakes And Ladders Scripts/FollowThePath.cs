using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FollowThePath : MonoBehaviour {

    //public Transform[] waypoints;
    public List<GameObject> waypoints = new List<GameObject>();

    [SerializeField]
    private float moveSpeed = 1f;
    private float moveH, moveV;

    [HideInInspector]
    public int waypointIndex = 0;
    public bool moveAllowed = false;

    // Use this for initialization
    private void Start () {

        /* foreach(GameObject fooObj in GameObject.FindGameObjectsWithTag("WayPoint")){
			waypoints.Add(fooObj);
		}
        waypoints.Reverse(); */
        transform.position = waypoints[waypointIndex].transform.position;



       
	}
	
	// Update is called once per frame
	private void Update () {
        
        if (moveAllowed){
            moveH = waypoints[waypointIndex].transform.position.x - transform.position.x * moveSpeed;
            moveV = waypoints[waypointIndex].transform.position.y - transform.position.y * moveSpeed;

            Vector2 direction = new Vector2(moveH, moveV);

            FindObjectOfType<PlayerAnimation>().SetDirection(direction);
            Move();
        }
        
	}

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

