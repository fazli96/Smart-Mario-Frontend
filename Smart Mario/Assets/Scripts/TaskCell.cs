using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskCell : MonoBehaviour
{
    [SerializeField]
    private int GameID;
    [SerializeField]
    private string Difficulty;
    [SerializeField]
    private string Level;
    [SerializeField]
    private string CompletionStatus;

    public Text GameIDText;
    public Text DifficultyText;
    public Text LevelText;
    public Text CompletionStatusText;

    public void SetCell(int gameID, string difficultyText, string levelText, string completionStatus)
    {
        GameID = gameID;
        GameIDText.text = gameID.ToString();
        Difficulty = difficultyText;
        DifficultyText.text = difficultyText;
        Level = levelText;
        LevelText.text = levelText;
        CompletionStatus = completionStatus;
        CompletionStatusText.text = completionStatus;
    }
}
