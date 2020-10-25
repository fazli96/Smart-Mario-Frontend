using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskCell : MonoBehaviour
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

    public Text gameNameText;
    public Text difficultyText;
    public Text levelText;
    public Text createdAtText;
    public Text completionStatusText;

    public void SetCell(string gameName, string difficultyText, string levelText, string createdAt, string completionStatus)
    {
        this.gameName = gameName;
        this.gameNameText.text = gameName;
        this.difficulty = difficultyText;
        this.difficultyText.text = difficultyText;
        this.level = levelText;
        this.levelText.text = levelText;
        this.createdAt = createdAt;
        this.createdAtText.text = createdAt;
        this.completionStatus = completionStatus;
        this.completionStatusText.text = completionStatus;
    }
}
