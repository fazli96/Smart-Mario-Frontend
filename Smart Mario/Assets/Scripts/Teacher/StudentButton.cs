using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Game object student button
/// </summary>
public class StudentButton : MonoBehaviour
{
    [SerializeField]
    private string name;
    private string studentId;
    public Text ButtonText;
    public StudentScrollViewController ScrollView;

    /// <summary>
    /// Set the attributes of the button
    /// </summary>
    /// <param name="name"></param>
    /// <param name="studentId"></param>
    public void SetAttributes(string name, string studentId)
    {
        this.name = name;
        ButtonText.text = name;
        this.studentId = studentId;
    }

    /// <summary>
    /// Action when the button is clicked; directs the user to the statistics page of the user.
    /// </summary>
    public void ButtonClick()
    {
        StatisticsManager statisticsManager = StatisticsManager.GetStatisticsManager();
        statisticsManager.SetStudentAttributes(name, studentId, true);
        ScrollView.ButtonClicked();
    }
}
