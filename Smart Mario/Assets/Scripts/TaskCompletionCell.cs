using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCompletionCell : MonoBehaviour
{
    [SerializeField]
    private int GameID;
    [SerializeField]
    private string Difficulty;
    [SerializeField]
    private string Level;
    [SerializeField]
    private string CompletionStatus;
    
    public TaskCompletionCell(int gameID, string difficulty, string level, string completionStatus)
    {
        GameID = gameID;
        Difficulty = difficulty;
        Level = level;
        CompletionStatus = completionStatus;
    }

    public int GetGameID() { return GameID; }

    public string GetDifficulty() { return Difficulty; }

    public string GetLevel() { return Level; }

    public string GetCompletionStatus() { return CompletionStatus; }
}
