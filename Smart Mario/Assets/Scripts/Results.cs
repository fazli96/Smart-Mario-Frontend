using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Results 
{
    public string studentId;
    public int minigameId;
    public string difficulty;
    public int level;
    public int score;
    public int questions_attempted;
    public int questions_correct;

    public Results(string studentId, int minigameId, string difficulty, int level, int score, int questions_attempted, int questions_correct)
    {
        this.studentId = studentId;
        this.minigameId = minigameId;
        this.difficulty = difficulty;
        this.level = level;
        this.score = score;
        this.questions_attempted = questions_attempted;
        this.questions_correct = questions_correct;
    }

    public static Results CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<Results>(data);
    }
}
