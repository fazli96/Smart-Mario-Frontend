using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeText1 : MonoBehaviour
{
    public GameObject questionManager;
    Text question;
    void Start()
    {
        question = GetComponent<Text>();
    }

    public void change(int num)
    {
        question.text = num.ToString(); 
    }
}
