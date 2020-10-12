using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class manages the cards played in the game and instantiates and stores them in a list datastructure
/// </summary>
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
    /// <summary>
    /// Called at the initialisation of this script
    /// Instantiates the appropriate number of cards and at the right coordinates in accordance to the difficulty level
    /// </summary>
    private void Awake()
    {
        faceindexes.Clear();
        pairs = 0;
        string difficulty = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        int level = PlayerPrefs.GetInt("MinigameLevel", 1);
        level = 5;  //hardcode level here
        switch (level)
        {
            case 1:
                pairs = 4;
                break;
            case 2:
                pairs = 6;
                break;
            case 3:
                pairs = 9;
                break;
            case 4:
                pairs = 12;
                break;
            case 5:
                pairs = 12;
                break;
            default:
                pairs = 4;
                break;
        }
        //switch (difficulty)
        //{
        //    case "Easy":
        //    case "Medium":
        //    case "Hard":
        //}
        for (int k = 0; k < 2; k++)
        {
            for (int i = 0; i < pairs; i++)
            {
                faceindexes.Add(i);
                UnityEngine.Debug.Log(i);
            }
        }
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
            else if (pairs == 12)
            {
                yPosition = 10.5f;
                xPosition = -19.5f;
            }
        }
        Debug.Log("init pairs " + pairs);
        for (int j = 0; j < pairs*2; j++)
        {
            shuffleNum = rnd.Next(0, faceindexes.Count);      //randomising faceindex
            var temp = Instantiate(token, new Vector3(          //instantiating prefab
                xPosition, yPosition, 0),
                Quaternion.identity);
            var current = temp.GetComponent<CardControl>().faceIndex = faceindexes[shuffleNum];   //setting faceindex of the prefab 
            var count = 0;
            foreach (int i in faceindexes)
            {
                if (i != null && i == current) count++;
            }
            temp.GetComponent<CardControl>().qOrA = count;
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
            else if (pairs == 12)
            {
                if (j == 5)
                {
                    yPosition = 5.5f;
                    xPosition = -19.5f;
                }
                else if (j == 11)
                {
                    yPosition = 0.5f;
                    xPosition = -19.5f;
                }
                else if (j == 17)
                {
                    yPosition = -4.5f;
                    xPosition = -19.5f;
                }
            }

        }
        faceindexes.Clear();
    }

    /// <summary>
    /// This is called when a card is opened up
    /// Invokes a function for the specific card that is being opened
    /// </summary>
    /// <param name="index"></param>
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
