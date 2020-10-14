using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckAssignedTasks : MonoBehaviour
{
    private SceneController scene;
    public Button backButton;

    private SelectTaskManager selectTaskManager;

    public Text gameNameText;
    public Text difficultyText;
    public Text levelText;

    public string gameName;
    public string difficulty;
    public string level;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        selectTaskManager = SelectTaskManager.GetSelectTaskManager();
        
        SetAttributes();

        DisplayAttributes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void BackToTeacherMenu()
    {
        scene.ToTeacherSelectTaskScreen();
    }

    public void DisplayAttributes()
    {
        gameNameText.text = gameName;
        difficultyText.text = difficulty;
        levelText.text = level;
    }

    public void SetAttributes()
    {
        this.gameName = selectTaskManager.GetGameID();
        this.difficulty = selectTaskManager.GetDifficulty();
        this.level = selectTaskManager.GetLevel();
    }
}
