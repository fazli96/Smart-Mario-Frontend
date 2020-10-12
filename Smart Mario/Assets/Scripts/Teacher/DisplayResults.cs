using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayResults
{
    public int id;
    public int minigameId;
    public int studentId;
    public int teacherId;
    public string difficulty;
    public string level;
    public string completed;
    public string createdAt;
    public string updatedAt;
    public List<StudentResult> studentResultList;

    public DisplayResults(int id, int minigameId, int studentId, int teacherId, string difficulty, string level, string completed, string createdAt, string updatedAt, List<StudentResult> studentResultList)
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
        this.studentResultList = studentResultList;
    }
}
