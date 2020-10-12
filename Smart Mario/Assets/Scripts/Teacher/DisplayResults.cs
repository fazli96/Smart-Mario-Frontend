using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayResults
{
    public string id;
    public string minigameId;
    public string studentId;
    public string teacherId;
    public string difficulty;
    public string level;
    public string completed;
    public string createdAt;
    public string updatedAt;

    public DisplayResults(string id, string minigameId, string studentId, string teacherId, string difficulty, string level, string completed, string createdAt, string updatedAt)
    {
        this.id = id;
        this.minigameId = minigameId;
        this.studentId = studentId;
        this.teacherId = teacherId;
        this.difficulty = difficulty;
        this.level = level;
        this.completed = completed;
        this.createdAt = createdAt;
        this.updatedAt = updatedAt;
    }
}
