using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentButton : MonoBehaviour
{
    [SerializeField]
    private string Name;
    public Text ButtonText;
    public StudentScrollViewController ScrollView;

    public void SetName(string name)
    {
        Name = name;
        ButtonText.text = name;
    }

    public void ButtonClick()
    {
        ScrollView.ButtonClicked(Name);
    }
}
