using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    GameObject questionManager;
    GameObject cardManager;
    GameObject canvas;
    public GameObject token;
    public GameObject finishText;
    public GameObject overlay;
    public List<int> faceindexes = new List<int> { 0, 1, 2, 3, 0, 1, 2, 3 };
    public List<GameObject> cards = new List<GameObject>();
    public static System.Random rnd = new System.Random();
    public int shuffleNum = 0;
    int[] visibleFaces = { -1, -2 };
    public int pairs = 4;
    public int scoreValue = 0;

    private void Start()
    {
        finishText.SetActive(false);
        overlay.SetActive(false);
        int originalLength = faceindexes.Count;
        float yPosition = 6f;
        float xPosition = -14.5f;
        for (int i= 0; i< 8; i++)
        {
            shuffleNum = rnd.Next(0, (faceindexes.Count));      //randomising faceindex
            var temp = Instantiate(token, new Vector3(          //instantiating prefab
                xPosition, yPosition, 0),
                Quaternion.identity);
            temp.GetComponent<CardControl>().faceIndex = faceindexes[shuffleNum];   //setting faceindex of the prefab 
            cardManager.GetComponent<CardsManager>().AddObject(temp);
            faceindexes.Remove(faceindexes[shuffleNum]);        //removing this faceindex from available pool
            xPosition = xPosition + 5;                          //changing position of next instantiations
            if (i == (originalLength/2 -1))
            {
                yPosition = 0.75f;
                xPosition = -14.5f;
            }

        }
        //token.GetComponent<NewBehaviourScript>().faceIndex = faceindexes[0];
        faceindexes.Clear();
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
        faceindexes.Add(index);
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
            pairs--;
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
