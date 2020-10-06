using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class Game2Control : MonoBehaviour
{
    GameObject questionManager;
    GameObject cardManager;
    GameObject canvas;
    int[] visibleFaces = { -1, -2 };
    public GameObject finishText;
    public GameObject overlay;
    
    public int scoreValue = 0;

    private void Start()
    {
        finishText.SetActive(false);
        overlay.SetActive(false);
        
    }
    public bool TwoCards()
    {
    
        return (visibleFaces[0] >= 0 && visibleFaces[1] >= 0);

    }
    public void AddVisibleFace(int index)
    {
        if (visibleFaces[0]== -1)
        {
            visibleFaces[0] = index;
            questionManager.GetComponent<questionManager>().showQuestion(1, index);
        }
        else if (visibleFaces[1] == -2)
        {
            visibleFaces[1] = index;
            questionManager.GetComponent<questionManager>().showQuestion(2, index);
        }
    }

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
    public void ShowMatch(int index)
    {
        cardManager.GetComponent<CardsManager>().faceindexes.Add(index);
        cardManager.GetComponent<CardsManager>().Open(index);
        canvas.GetComponent<CanvasControl>().ShowMatch();
        questionManager.GetComponent<questionManager>().hideQuestion(1, true );
        questionManager.GetComponent<questionManager>().hideQuestion(2, true );
    }
   
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
    private void Awake()
    {
        questionManager = GameObject.Find("QuestionManager");
        cardManager = GameObject.Find("CardsManager");
        canvas = GameObject.Find("Canvas");
    }

}
