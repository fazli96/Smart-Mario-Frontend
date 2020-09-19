using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveH, moveV;
    [SerializeField] private float moveSpeed = 1.0f;

    //network
    private bool isLocalPlayer = true; //switch back to false when networking
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
            return;
        }

        moveH = Input.GetAxis("Horizontal") * moveSpeed;
        moveV = Input.GetAxis("Vertical") * moveSpeed;
        rb.velocity = new Vector2(moveH, moveV);//OPTIONAL rb.MovePosition();

        Vector2 direction = new Vector2(moveH, moveV);

        if (currentPosition != oldPosition)
        {
            //TODO networking
            oldPosition = currentPosition;
        }

        FindObjectOfType<PlayerAnimation>().SetDirection(direction);
    }



}
