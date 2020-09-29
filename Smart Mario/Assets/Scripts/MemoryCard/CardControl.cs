using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardControl : MonoBehaviour
{
    GameObject GameControl;
    SpriteRenderer spriteRenderer;
    public Sprite[] faces;
    public Sprite back;
    public Sprite open;
    public int faceIndex;
    public bool matched = false;

    public void OnMouseDown()       //detecting mouseclick
    {
        if (matched == false && !GameControl.GetComponent<Game2Control>().faceindexes.Contains(faceIndex)) //not matched and not flipped up
        {
            if (spriteRenderer.sprite == back)    //check if its back of the card
            {
                if (GameControl.GetComponent<Game2Control>().TwoCards() == false) //if less than two cards have been flipped up
                {      
                    spriteRenderer.sprite = open;               //reveal the inside sprite 
                    GameControl.GetComponent<Game2Control>().AddVisibleFace(faceIndex);   //add the index of the face of this card to arr
                    matched = GameControl.GetComponent<Game2Control>().CheckMatch();     //check if got match 
                    if (matched)
                    {
                        GameControl.GetComponent<Game2Control>().ShowMatch(faceIndex);
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
    public void Change()
    {
        spriteRenderer.sprite = faces[faceIndex];       //reveal the true sprite
    }
    
    private void Awake()
    {

        GameControl = GameObject.Find("GameManager");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
