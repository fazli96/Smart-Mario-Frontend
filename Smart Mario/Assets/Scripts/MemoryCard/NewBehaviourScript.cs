using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    GameObject GameControl;
    SpriteRenderer spriteRenderer;
    public Sprite[] faces;
    public Sprite back;
    public int faceIndex;
    public bool matched = false;

    public void OnMouseDown()       //detecting mouseclick
    {
        if (matched == false && !GameControl.GetComponent<GameControl>().faceindexes.Contains(faceIndex)) //not matched and not flipped up
        {
            if (spriteRenderer.sprite == back)    //check if its back of the card
            {
                if (GameControl.GetComponent<GameControl>().TwoCards() == false) //if less than two cards have been flipped up
                {
                    spriteRenderer.sprite = faces[faceIndex];       //add the index of the face of this card to arr
                    //add the faceIndex to the UI element
                    GameControl.GetComponent<GameControl>().AddVisibleFace(faceIndex);  
                    matched = GameControl.GetComponent<GameControl>().CheckMatch();     //check if got match 
                    if (matched)
                    {
                        GameControl.GetComponent<GameControl>().faceindexes.Add(faceIndex); 
                    }
                }
            }
            else
            {
                spriteRenderer.sprite = back;   //flip back the card onto backside
                GameControl.GetComponent<GameControl>().RemoveVisibleFace(faceIndex);  //remove the face

            }
        }
        
    }
    
    private void Awake()
    {
        GameControl = GameObject.Find("Control");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
