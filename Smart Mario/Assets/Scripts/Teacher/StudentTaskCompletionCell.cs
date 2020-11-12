using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Game object which displays the student's task completion status in the check assigned tasks scroll view
/// </summary>
public class StudentTaskCompletionCell : MonoBehaviour
{
    [SerializeField]
    private string Name;
    [SerializeField]
    private string CompletionStatus;
    
    public Text CellNameText;
    public Text CellCompletionStatusText;
    public StudentScrollViewController ScrollView;

    /// <summary>
    /// Set the attributes of the cell to be displayed.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="completionStatus"></param>
    public void SetCell(string name, string completionStatus)
    {
        Name = name;
        CellNameText.text = name;
        CompletionStatus = completionStatus;
        CellCompletionStatusText.text = completionStatus;
    }
}
