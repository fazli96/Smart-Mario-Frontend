using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Results 
{
    public string studentId;
    public string minigameId;
    public string difficulty;
    public string level;
    public string score;
    public string questions_attempted;
    public string questions_correct;

    public Results(string studentId, string minigameId, string difficulty, string level, string score, string questions_attempted, string questions_correct)
    {
        this.studentId = studentId;
        this.minigameId = minigameId;
        this.difficulty = difficulty;
        this.level = level;
        this.score = score;
        this.questions_attempted = questions_attempted;
        this.questions_correct = questions_correct;
    }
}
