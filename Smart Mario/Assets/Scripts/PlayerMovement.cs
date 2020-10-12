using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
/// <summary>
/// This class is for the player movement on the world map via arrow keys
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveH, moveV;
    [SerializeField] 
    private float moveSpeed = 1.0f;
    private PlayerAnimation anim;

    //network
    public bool multiplayer = false; //switch back to false when networking
    public bool isLocalPlayer = true; //switch back to false when networking
    public bool isOwner = false;
    Vector3 oldPosition;
    Vector3 currentPosition;

    /// <summary>
    /// This is used for initialization
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        oldPosition = transform.position;
        currentPosition = oldPosition;
        Transform p = transform.Find("Player");
        anim = p.GetComponent<PlayerAnimation>();
    }

    /// <summary>
    /// Update is called for every frame
    /// </summary>
    private void Update()
    {
        if (isOwner && isLocalPlayer)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 1)
            {
                RoomManager.instance.GetComponent<RoomManager>().ShowStartChallengeButton();
            }
            else
            {
                RoomManager.instance.GetComponent<RoomManager>().DisableStartChallengeButton();
            }
        }
        if (!isLocalPlayer)
        {
            
            if (currentPosition != transform.position)
            {
                Debug.LogError("notLocalPlayer"+transform.name + isLocalPlayer + " " + isOwner + " " + multiplayer);

                moveH = transform.position.x - currentPosition.x;
                Debug.Log("moveH: " + moveH + " = " + currentPosition.x + " - " + oldPosition.x);
                
                moveV = transform.position.y - currentPosition.y;
                Debug.Log("moveV: " + moveV + " = " + currentPosition.y + " - " + oldPosition.y);

                Vector2 direction = new Vector2(moveH, moveV);

                anim.SetDirection(direction);
                currentPosition = transform.position;
            }
        }
        else
        {
            
            moveH = Input.GetAxis("Horizontal") * moveSpeed;
            moveV = Input.GetAxis("Vertical") * moveSpeed;
            rb.velocity = new Vector2(moveH, moveV);//OPTIONAL rb.MovePosition();

            Vector2 direction = new Vector2(moveH, moveV);

            currentPosition = transform.position;

            if (currentPosition != oldPosition && multiplayer)
            {
                Debug.LogError("isLocalPlayer" + transform.name + isLocalPlayer + " " + isOwner + " " + multiplayer);
                NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(transform.position);
                oldPosition = currentPosition;
            }
            anim.SetDirection(direction);
        }


    }

    /// <summary>
    /// This is to avoid player to player collision during multiplayer
    /// </summary>
    private void OnEnable()
    {
        GameObject[] otherObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in otherObjects)
        {
            Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

    }



}
