using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CardsManager: MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();
    
    public void AddObject(GameObject card)
    {
        cards.Add(card);
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
