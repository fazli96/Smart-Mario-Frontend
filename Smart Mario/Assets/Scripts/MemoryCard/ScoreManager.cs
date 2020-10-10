using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is updates the score of the card game based on the state stored in Game2Control
/// </summary>
public class ScoreManager: MonoBehaviour
{
    public GameObject GameControl;
    Text score;
    /// <summary>
    /// This is called at the start of script initialisation
    /// </summary>
    void Start()
    {
        score = GetComponent<Text>();
    }
   /// <summary>
   /// This is called which each update of frames
   /// It updates the score counter
   /// </summary>
    void Update()
    {
        score.text = "Score : " + GameControl.GetComponent<Game2Control>().scoreValue;
    }
}
