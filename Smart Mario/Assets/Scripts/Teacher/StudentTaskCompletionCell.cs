using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentTaskCompletionCell : MonoBehaviour
{
    [SerializeField]
    private string Name;
    [SerializeField]
    private bool CompletionStatus;
    
    public Text CellNameText;
    public Text CellCompletionStatusText;
    public StudentScrollView ScrollView;

    public void SetCell(string name, bool completionStatus)
    {
        Name = name;
        CellNameText.text = name;
        CompletionStatus = completionStatus;
        CellCompletionStatusText.text = completionStatus ? "Completed" : "Incomplete";
    }
}
