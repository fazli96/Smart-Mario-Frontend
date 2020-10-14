using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentTaskCompletionCell : MonoBehaviour
{
    [SerializeField]
    private string Name;
    [SerializeField]
    private string CompletionStatus;
    
    public Text CellNameText;
    public Text CellCompletionStatusText;
    public StudentScrollView ScrollView;

    public void SetCell(string name, string completionStatus)
    {
        Name = name;
        CellNameText.text = name;
        CompletionStatus = completionStatus;
        CellCompletionStatusText.text = completionStatus;
    }
}
