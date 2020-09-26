using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveH, moveV;
    [SerializeField] 
    private float moveSpeed = 1.0f;

    //network
    public bool isLocalPlayer = true; //switch back to false when networking
    Vector3 oldPosition;
    Vector3 currentPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        oldPosition = transform.position;
        currentPosition = oldPosition;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            currentPosition = transform.position;

            if (currentPosition != oldPosition)
            {

                moveH = currentPosition.x - oldPosition.x;
                Debug.Log("moveH: " + moveH + " = " + currentPosition.x + " - " + oldPosition.x);
                
                moveV = currentPosition.y - oldPosition.y;
                Debug.Log("moveV: " + moveV + " = " + currentPosition.y + " - " + oldPosition.y);

                Vector2 direction = new Vector2(moveH, moveV);

                Transform p = transform.Find("Witch Sprite");
                PlayerAnimation anim = p.GetComponent<PlayerAnimation>();
                anim.SetDirection(direction);
                oldPosition = currentPosition;
            }
        }
        else
        {
            moveH = Input.GetAxis("Horizontal") * moveSpeed;
            moveV = Input.GetAxis("Vertical") * moveSpeed;
            rb.velocity = new Vector2(moveH, moveV);//OPTIONAL rb.MovePosition();

            Vector2 direction = new Vector2(moveH, moveV);

            currentPosition = transform.position;

            if (currentPosition != oldPosition)
            {
                NetworkManager.instance.GetComponent<NetworkManager>().CommandMove(transform.position);
                oldPosition = currentPosition;
            }
            Transform p = transform.Find("Witch Sprite");
            PlayerAnimation anim = p.GetComponent<PlayerAnimation>();
            anim.SetDirection(direction);
        }


    }

    private void OnEnable()
    {
        GameObject[] otherObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in otherObjects)
        {
            Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

    }



}
