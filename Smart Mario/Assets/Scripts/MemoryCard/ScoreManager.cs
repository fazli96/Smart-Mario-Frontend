using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager: MonoBehaviour
{
    public GameObject GameControl;
    Text score;
    void Start()
    {
        score = GetComponent<Text>();
    }
   
    void Update()
    {
        score.text = "Score : " + GameControl.GetComponent<GameControl>().scoreValue;
    }
}
