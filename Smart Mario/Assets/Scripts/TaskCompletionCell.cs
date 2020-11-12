using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which contains all the information which will be displayed to the user.
/// </summary>
public class TaskCompletionCell : MonoBehaviour
{
    [SerializeField]
    private string gameName;
    [SerializeField]
    private string difficulty;
    [SerializeField]
    private string level;
    [SerializeField]
    private string createdAt;
    [SerializeField]
    private string completionStatus;
    
    /// <summary>
    /// Set the attributes of the class
    /// </summary>
    /// <param name="gameName"></param>
    /// <param name="difficulty"></param>
    /// <param name="level"></param>
    /// <param name="createdAt"></param>
    /// <param name="completionStatus"></param>
    public TaskCompletionCell(string gameName, string difficulty, string level, string createdAt, string completionStatus)
    {
        this.gameName = gameName;
        this.difficulty = difficulty;
        this.level = level;
        this.createdAt = createdAt;
        this.completionStatus = completionStatus;
    }

    /// <summary>
    /// Function call to get the name to be displayed
    /// </summary>
    /// <returns></returns>
    public string GetGameName() { return gameName; }

    /// <summary>
    /// Function call to get the difficulty to be displayed
    /// </summary>
    /// <returns></returns>
    public string GetDifficulty() { return difficulty; }

    /// <summary>
    /// Function call to get the level to be displayed
    /// </summary>
    /// <returns></returns>
    public string GetLevel() { return level; }

    /// <summary>
    /// Function call to get the creation time to be displayed
    /// </summary>
    /// <returns></returns>
    public string GetCreatedAt() { return createdAt; }

    /// <summary>
    /// Function call to get the completion status to be displayed
    /// </summary>
    /// <returns></returns>
    public string GetCompletionStatus() { return completionStatus; }
}
