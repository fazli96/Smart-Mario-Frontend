using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentButton : MonoBehaviour
{
    [SerializeField]
    private string name;
    private string studentId;
    public Text ButtonText;
    public StudentScrollViewController ScrollView;

    public void SetAttributes(string name, string studentId)
    {
        this.name = name;
        ButtonText.text = name;
        this.studentId = studentId;
    }

    public void ButtonClick()
    {
        StudentPerformanceManager studentPerformanceManager = StudentPerformanceManager.GetStudentPerformanceManager();
        studentPerformanceManager.SetStudentAttributes(name, studentId);
        ScrollView.ButtonClicked();
    }
}
