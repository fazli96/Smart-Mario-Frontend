using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class controls the behavior of the canvas  
/// </summary>
public class CanvasControl : MonoBehaviour
{
    public Text matchText;
    Canvas canvas;
    /// <summary>
    /// This is called at the start of initialisation
    /// </summary>
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    /// <summary>
    /// This class instantiates text that shows a match between two cards
    /// </summary>
    public void ShowMatch()
    {
        Text one = Instantiate(matchText, new Vector2(          //instantiating prefab
                350, 90),
                Quaternion.identity);
        one.transform.SetParent(canvas.transform, false);
        Text two = Instantiate(matchText, new Vector2(          //instantiating prefab
                350, -40),
                Quaternion.identity);
        two.transform.SetParent(canvas.transform, false);
        
    }
}
