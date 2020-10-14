using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCompletionCell : MonoBehaviour
{
    [SerializeField]
    private string gameName;
    [SerializeField]
    private string difficulty;
    [SerializeField]
    private string level;
    [SerializeField]
    private string completionStatus;
    
    public TaskCompletionCell(string gameName, string difficulty, string level, string completionStatus)
    {
        this.gameName = gameName;
        this.difficulty = difficulty;
        this.level = level;
        this.completionStatus = completionStatus;
    }

    public string GetGameName() { return gameName; }

    public string GetDifficulty() { return difficulty; }

    public string GetLevel() { return level; }

    public string GetCompletionStatus() { return completionStatus; }
}
