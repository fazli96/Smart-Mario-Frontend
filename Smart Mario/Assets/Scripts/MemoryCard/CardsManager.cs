using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CardsManager: MonoBehaviour
{
    public GameObject token;
    public List<int> faceindexes = new List<int>() { };
    public static System.Random rnd = new System.Random();
    public int shuffleNum = 0;
    public int pairs;
    float xPosition;
    float yPosition;
    public List<GameObject> cards = new List<GameObject>();
    public void Start()
    {
        faceindexes.Clear();
        pairs = 0;
        string difficulty = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        switch (difficulty)
        {
            case "Easy":
                pairs = 4;
                break;
            case "Medium":
                pairs = 6;
                break;
            case "Hard":
                pairs = 9;
                break;
            default:
                pairs = 4;
                break;
        }
        for (int k = 0; k < 2; k++)
        {
            for (int i = 0; i < pairs; i++)
            {
                faceindexes.Add(i);
                UnityEngine.Debug.Log(i);
            }
        }
        //UnityEngine.Debug.Log("pairs " + pairs);
        //UnityEngine.Debug.Log("elements are:");
        //foreach (var i in faceindexes)
        //{
        //    UnityEngine.Debug.Log(i);
        //}
        //int originalLength = pairs*2;
        if (pairs == 4)
        {
            yPosition = 6.5f;
            xPosition = -14.5f;
        }
        else
        {
            if (pairs == 6)
            {
                yPosition = 8.5f;
                xPosition = -14.5f;
            }
            else if (pairs == 9)
            {
                yPosition = 8.5f;
                xPosition = -19.5f;
            }
        }
        for (int j = 0; j < pairs*2; j++)
        {
            shuffleNum = rnd.Next(0, faceindexes.Count);      //randomising faceindex
            var temp = Instantiate(token, new Vector3(          //instantiating prefab
                xPosition, yPosition, 0),
                Quaternion.identity);
            temp.GetComponent<CardControl>().faceIndex = faceindexes[shuffleNum];   //setting faceindex of the prefab 
            UnityEngine.Debug.Log("set index:" + faceindexes[shuffleNum]);
            cards.Add(temp);
            faceindexes.Remove(faceindexes[shuffleNum]);        //removing this faceindex from available pool
            xPosition = xPosition + 5;                          //changing position of next instantiations
            if (pairs == 4)
            {
                if(j== pairs-1)
                    {
                        yPosition = 1f;
                        xPosition = -14.5f;
                    }
            }
            else if (pairs == 6)
            {
                if(j == 3)
                {
                    yPosition = 3.5f;
                    xPosition = -14.5f;
                }
                else if (j == 7)
                { 
                    yPosition = -1.5f;
                    xPosition = -14.5f;
                }
            }
            else if ( pairs == 9)
            {
                if(j == 5)
                {
                    yPosition = 3.5f;
                    xPosition = -19.5f;
                }
                else if (j == 11)
                {
                    yPosition = -1.5f;
                    xPosition = -19.5f;
                }
            }

        }
        //token.GetComponent<NewBehaviourScript>().faceIndex = faceindexes[0];
        faceindexes.Clear();
    }

  
    public void Open(int index)
    {
        foreach (var i in cards)
        {
            if (i.GetComponent<CardControl>().faceIndex == index)
            {
                i.GetComponent<CardControl>().Change();
            }
            UnityEngine.Debug.Log(i.GetComponent<CardControl>().faceIndex);
        }
    }
}
