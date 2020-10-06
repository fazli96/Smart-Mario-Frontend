﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the behavior of a specific card in the game
/// </summary>
public class CardControl : MonoBehaviour
{
    GameObject GameControl;
    SpriteRenderer spriteRenderer;
    GameObject cardsManager;
    public Sprite[] faces;
    public Sprite back;
    public Sprite open;
    public int faceIndex;
    public int qOrA;
    public bool matched = false;
    /// <summary>
    /// This is called when the individual cards detects a mouseclick
    /// Several checking mechanisms are donee
    /// </summary>
    public void OnMouseDown()       //detecting mouseclick
    {
        if (matched == false && !cardsManager.GetComponent<CardsManager>().faceindexes.Contains(faceIndex)) //not matched and not flipped up
        {
            if (spriteRenderer.sprite == back)    //check if its back of the card
            {
                if (GameControl.GetComponent<Game2Control>().TwoCards() == false) //if less than two cards have been flipped up
                {      
                    spriteRenderer.sprite = open;               //reveal the inside sprite 
                    GameControl.GetComponent<Game2Control>().AddVisibleFace(faceIndex,qOrA);   //add the index of the face of this card to arr
                    matched = GameControl.GetComponent<Game2Control>().CheckMatch();     //check if got match 
                    if (matched)
                    {
                        GameControl.GetComponent<Game2Control>().ShowMatch(faceIndex);   //reveal inside treasure
                    }
                    
                }
            }
            else
            {
                spriteRenderer.sprite = back;   //flip back the card onto backside
                GameControl.GetComponent<Game2Control>().RemoveVisibleFace(faceIndex);  //remove the face

            }
        }
        
    }
    /// <summary>
    /// This is called when there is a sucessful match of cards
    /// </summary>
    public void Change()
    {
        spriteRenderer.sprite = faces[faceIndex];       //reveal the true sprite
    }
    /// <summary>
    /// This is called before the first frame update
    /// </summary>
    private void Awake()
    {

        GameControl = GameObject.Find("GameManager");
        cardsManager = GameObject.Find("CardsManager");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
