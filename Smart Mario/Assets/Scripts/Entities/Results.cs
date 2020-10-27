using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
/// <summary>
/// Entity that stores information for results specific to student, minigame, difficulty and level
/// </summary>
public class Results 
{
    public string studentId;
    public int minigameId;
    public string difficulty;
    public int level;
    public int score;
    public int questions_attempted;
    public int questions_correct;
    public float time_taken;
    /// <summary>
    /// Constructor for Results object
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="minigameId"></param>
    /// <param name="difficulty"></param>
    /// <param name="level"></param>
    /// <param name="score"></param>
    /// <param name="questions_attempted"></param>
    /// <param name="questions_correct"></param>
    /// <param name="time_taken"></param>
    public Results(string studentId, int minigameId, string difficulty, int level, int score, int questions_attempted, int questions_correct, float time_taken = 0)
    {
        this.studentId = studentId;
        this.minigameId = minigameId;
        this.difficulty = difficulty;
        this.level = level;
        this.score = score;
        this.questions_attempted = questions_attempted;
        this.questions_correct = questions_correct;
        this.time_taken = time_taken;
    }

    /// <summary>
    /// Static method that takes queried Json data and converts it into a Results object
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Results CreateFromJSON(string data)
    {
        return JsonUtility.FromJson<Results>(data);
    }
}
