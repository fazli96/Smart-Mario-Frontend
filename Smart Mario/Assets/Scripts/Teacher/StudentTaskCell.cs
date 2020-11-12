using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An entity that contains the 2 string attributes which will be used to display to the user
/// </summary>
public class StudentTaskCell : MonoBehaviour
{
    [SerializeField]
    private string Name;
    [SerializeField]
    private string CompletionStatus;

    /// <summary>
    /// Set the attributes of the cell
    /// </summary>
    /// <param name="name"></param>
    /// <param name="completionStatus"></param>
    public StudentTaskCell(string name, string completionStatus)
    {
        Name = name;
        CompletionStatus = completionStatus;
    }

    /// <summary>
    /// Getter function for the student's name
    /// </summary>
    /// <returns></returns>
    public string GetName() { return Name; }

    /// <summary>
    /// Getter function for the task completion status
    /// </summary>
    /// <returns></returns>
    public string GetCompletionStatus() { return CompletionStatus; }
}
