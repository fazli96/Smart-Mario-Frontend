using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// This class checks the logic of minigame 2 as well as keeping track of the states in the game
/// </summary>
public class Game2Control : MonoBehaviour
{
    GameObject questionManager;
    GameObject cardManager;
    GameObject canvas;
    int[] visibleFaces = { -1, -2 };
    public GameObject finishText;
    public GameObject overlay;
    
    public int scoreValue = 0;
    /// <summary>
    /// Called at the start of script initialisation
    /// </summary>
    private void Start()
    {
        finishText.SetActive(false);
        overlay.SetActive(false);
        
    }
    /// <summary>
    /// Checks the state of the game whether two cards have been flipped up
    /// </summary>
    /// <returns>Boolean on whether there are two flipped cards</returns>
    public bool TwoCards()
    {
    
        return (visibleFaces[0] >= 0 && visibleFaces[1] >= 0);

    }
    /// <summary>
    /// Adds a face to a list of already visible faces
    /// Called when a card has been flipped up
    /// Subsequently shows the questions corresponding to the card
    /// </summary>
    /// <param name="index"></param>
    /// <param name="qOrA"></param>
    public void AddVisibleFace(int index, int qOrA)
    {
        if (visibleFaces[0]== -1)
        {
            visibleFaces[0] = index;
            questionManager.GetComponent<questionManager>().showQuestion(1, index, qOrA);
        }
        else if (visibleFaces[1] == -2)
        {
            visibleFaces[1] = index;
            questionManager.GetComponent<questionManager>().showQuestion(2, index, qOrA);
        }
    }
    /// <summary>
    /// Removes a face from the list of visible faces
    /// Also hides the corresponding question of the hidden card
    /// </summary>
    /// <param name="index"></param>
    public void RemoveVisibleFace(int index)
    {
        if (visibleFaces[0] == index)
        {
            visibleFaces[0] = -1;
            questionManager.GetComponent<questionManager>().hideQuestion(1, false);
        }
        else if (visibleFaces[1] == index)
        {
            visibleFaces[1] = -2;
            questionManager.GetComponent<questionManager>().hideQuestion(2, false);
        }
    }
    /// <summary>
    /// Called when there is match between two cards
    /// </summary>
    /// <param name="index"></param>
    public void ShowMatch(int index)
    {
        cardManager.GetComponent<CardsManager>().faceindexes.Add(index);
        cardManager.GetComponent<CardsManager>().Open(index);
        canvas.GetComponent<CanvasControl>().ShowMatch();
        questionManager.GetComponent<questionManager>().hideQuestion(1, true );
        questionManager.GetComponent<questionManager>().hideQuestion(2, true );
    }
   /// <summary>
   /// Checks whether two visible faces are matching or not
   /// </summary>
   /// <returns>Boolean on match of two cards</returns>
    public bool CheckMatch()
    {
        if (visibleFaces[0]== visibleFaces[1])
        {
            visibleFaces[0] = -1;
            visibleFaces[1] = -2;
            int pairs = (cardManager.GetComponent<CardsManager>().pairs) - 1;
            print(pairs);
            if (pairs == 0)
            {
                finishText.SetActive(true);
                overlay.SetActive(true);
            }
            scoreValue += 1; 
            
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Called before the first frame update
    /// </summary>
    private void Awake()
    {
        questionManager = GameObject.Find("QuestionManager");
        cardManager = GameObject.Find("CardsManager");
        canvas = GameObject.Find("Canvas");
    }

}
