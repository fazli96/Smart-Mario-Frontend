﻿using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour
{

    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private bool coroutineAllowed = true;


    // Use this for initialization
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rend.sprite = diceSides[5];
    }

    private void OnMouseDown()
    {
        if (!GameControl.levelComplete && coroutineAllowed 
            && !GameControl.getMoveAllowed() && !GameControl.qnEncountered)
            StartCoroutine("RollTheDice");
            //GetComponent<AudioSource>().Play();
    }

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
