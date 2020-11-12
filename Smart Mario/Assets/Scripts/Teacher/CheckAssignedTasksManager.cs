using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller class for check assigned tasks scene
/// </summary>
public class CheckAssignedTasksManager : MonoBehaviour
{
    private SceneController scene;
    public Button backButton;

    private SelectTaskManager selectTaskManager;
    private Translator translator = Translator.GetTranslator();

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
    
    /// <summary>
    /// Back button action
    /// Redirects user back to main menu
    /// </summary>
    public void BackToTeacherMenu()
    {
        scene.ToTeacherSelectTaskScreen();
    }

    /// <summary>
    /// Display the selected game, level and difficulty on the screen
    /// </summary>
    public void DisplayAttributes()
    {
        gameNameText.text = gameName;
        difficultyText.text = difficulty;
        levelText.text = level;
    }

    /// <summary>
    /// Function to retrieve the selected values
    /// </summary>
    public void SetAttributes()
    {
        this.gameName = translator.GameIDToName(selectTaskManager.GetMiniGameID());
        this.difficulty = selectTaskManager.GetDifficulty();
        this.level = "Level " + selectTaskManager.GetLevel();
    }
}
