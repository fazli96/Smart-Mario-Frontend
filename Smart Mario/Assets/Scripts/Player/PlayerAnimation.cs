using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class contains all methods to manage the movement animation of the player
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    public string[] staticDirections = { "Static N", "Static NW", "Static W", "Static SW", "Static S", "Static SE", "Static E", "Static NE" };
    public string[] runDirections = { "Run N", "Run NW", "Run W", "Run SW", "Run S", "Run SE", "Run E", "Run NE" };
    public bool isMoving;

    // ensure the player is facing south immediately after spawning
    int lastDirection = 4;

    /// <summary>
    /// This is called to initialize the player animation to the correct character animation
    /// </summary>
    private void Awake()
    {
        anim = GetComponent<Animator>();

        // below is for debug purposes to check the alignment of the vectors of player movement is isometric
        float result1 = Vector2.SignedAngle(Vector2.up, Vector2.right);
        Debug.Log("R1 " + result1);

        float result2 = Vector2.SignedAngle(Vector2.up, Vector2.left);
        Debug.Log("R2 " + result2);

        float result3 = Vector2.SignedAngle(Vector2.up, Vector2.down);
        Debug.Log("R3 " + result3);
    }

    //MARKER each direction will match with one string element
    //MARKER We used direction to determine their animation
    public void SetDirection(Vector2 _direction)
    {
        string[] directionArray = null;

        // Character is static. And his velocity is close to zero
        if (_direction.magnitude < 0.01)
        {
            directionArray = staticDirections;
        }
        else
        {
            directionArray = runDirections;
            // Get the index of the slice from the direction vector for isometric movement
            lastDirection = DirectionToIndex(_direction); 
        }

        anim.Play(directionArray[lastDirection]);
    }

    /// <summary>
    /// This class Converts a Vector2 direction to an index to a slice around a circle for isometric movement
    /// This goes in a counter-clock direction
    /// </summary>
    /// <param name="_direction"></param>
    /// <returns>an index to a slice around a circle</returns>
    private int DirectionToIndex(Vector2 _direction)
    {
        // return this vector with a magnitude of 1 and get the normalized to an index
        Vector2 norDir = _direction.normalized;

        // 45 one circle and 8 slices
        //Calculate how many degrees one slice is
        float step = 360 / 8;

        // help us easy to calcuate and get the correct index of the string array
        float offset = step / 2;

        // returns the signed angle in degrees between A and B
        float angle = Vector2.SignedAngle(Vector2.up, norDir);

        //Help us easy to calcuate and get the correct index of the string array
        angle += offset;

        //avoid the negative number
        if (angle < 0) 
        {
            angle += 360;
        }

        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }
}
