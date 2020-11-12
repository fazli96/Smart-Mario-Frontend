using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which contains the data of a student's progress / game
/// </summary>
public class DisplayResults
{
    public int id;
    public int minigameId;
    public int studentId;
    public int teacherId;
    public string studentName;
    public string difficulty;
    public string level;
    public string completed;
    public string createdAt;
    public string updatedAt;

    /// <summary>
    /// Set the attributes of this class
    /// </summary>
    /// <param name="id"></param>
    /// <param name="minigameId"></param>
    /// <param name="studentId"></param>
    /// <param name="teacherId"></param>
    /// <param name="difficulty"></param>
    /// <param name="level"></param>
    /// <param name="completed"></param>
    /// <param name="createdAt"></param>
    /// <param name="updatedAt"></param>
    public DisplayResults(int id, int minigameId, int studentId, int teacherId, string difficulty, string level, string completed, string createdAt, string updatedAt)
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
