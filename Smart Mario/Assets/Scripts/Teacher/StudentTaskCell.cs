using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentTaskCell : MonoBehaviour
{
    [SerializeField]
    private string Name;
    [SerializeField]
    private string CompletionStatus;

    public StudentTaskCell(string name, string completionStatus)
    {
        Name = name;
        CompletionStatus = completionStatus;
    }

    public string GetName() { return Name; }

    public string GetCompletionStatus() { return CompletionStatus; }
}
