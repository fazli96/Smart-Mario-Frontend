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

    //network
    public bool multiplayer = false; //switch back to false when networking
    public bool isLocalPlayer = true; //switch back to false when networking
    public bool isOwner = false;
    Vector3 oldPosition;
    Vector3 currentPosition;

    /// <summary>
    /// This is used for initialization
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        oldPosition = transform.position;
        currentPosition = oldPosition;
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
            currentPosition = transform.position;
            Transform p = transform.Find("Player");
            PlayerAnimation anim = p.GetComponent<PlayerAnimation>();

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
                anim.SetDirection(new Vector2(0, 0));
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
                NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(transform.position);
                oldPosition = currentPosition;
            }
            Transform p = transform.Find("Player");
            PlayerAnimation anim = p.GetComponent<PlayerAnimation>();
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
