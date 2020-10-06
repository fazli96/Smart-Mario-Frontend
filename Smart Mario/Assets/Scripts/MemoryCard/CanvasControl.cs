using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasControl : MonoBehaviour
{
    public Text matchText;
    Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowMatch()
    {
        Text one = Instantiate(matchText, new Vector2(          //instantiating prefab
                350, 90),
                Quaternion.identity);
        one.transform.SetParent(canvas.transform, false);
        //one.transform.parent = canvas.transform;
        Text two = Instantiate(matchText, new Vector2(          //instantiating prefab
                350, -40),
                Quaternion.identity);
        two.transform.SetParent(canvas.transform, false);
        
    }
}
