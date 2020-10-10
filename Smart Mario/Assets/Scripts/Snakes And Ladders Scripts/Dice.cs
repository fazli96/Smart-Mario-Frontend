﻿using System.Collections;
using UnityEngine;

/// <summary>
/// This class is for what happens when player rolls the dice
/// </summary>
public class Dice : MonoBehaviour
{

    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private bool coroutineAllowed = true;

    /// <summary>
    /// This is for initialization
    /// </summary>
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rend.sprite = diceSides[5];
    }
    /// <summary>
    /// Listens for mouse click down on the dice
    /// </summary>
    private void OnMouseDown()
    {
        if (!GameControl.levelComplete && coroutineAllowed 
            && !GameControl.GetMoveAllowed() && !GameControl.qnEncountered
            && GameControl.currentTurn)
            StartCoroutine("RollTheDice");
            //GetComponent<AudioSource>().Play();
    }
    /// <summary>
    /// A coroutine for rolling the dice. 
    /// Waits for the dice to finish rolling before the player moves
    /// </summary>
    /// <returns>wait for seconds</returns>
    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.1f);

        GameControl.diceSideThrown = randomDiceSide + 1;
        //randomDiceSide = 5 for six;

        GameControl.MovePlayer();

        coroutineAllowed = true;

    }
}
