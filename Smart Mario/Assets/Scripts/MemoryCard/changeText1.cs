using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class determines the behavior of the first question text
/// </summary>
public class changeText1 : MonoBehaviour
{
    public GameObject questionManager;
    Text question;
    /// <summary>
    /// Called at the start of the script initialisation
    /// </summary>
    void Start()
    {
        question = GetComponent<Text>();
    }
    /// <summary>
    /// This changes the text of the question
    /// </summary>
    /// <param name="num"></param>
    public void change(int num)
    {
        question.text = num.ToString(); 
    }
}
