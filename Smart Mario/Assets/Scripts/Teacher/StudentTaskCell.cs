using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentTaskCell : MonoBehaviour
{
    [SerializeField]
    private string Name;
    [SerializeField]
    private bool CompletionStatus;

    public StudentTaskCell(string name, bool completionStatus = false)
    {
        Name = name;
        CompletionStatus = completionStatus;
    }

    public string GetName() { return Name; }

    public bool GetCompletionStatus() { return CompletionStatus; }
}
