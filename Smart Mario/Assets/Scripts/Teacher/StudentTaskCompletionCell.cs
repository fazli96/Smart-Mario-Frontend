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

    public void SetName(string name)
    {
        Name = name;
        CellNameText.text = name;
    }

    public void SetCompletionStatus(bool completionStatus)
    {
        CompletionStatus = completionStatus;
        CellCompletionStatusText.text = completionStatus ? "Completed" : "Incomplete";
    }

    public void ButtonClick()
    {
        
    }
}
