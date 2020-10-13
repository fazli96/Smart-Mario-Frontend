using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CheckAssignedTasks : MonoBehaviour
{
    private SceneController scene;
    private static CheckAssignedTasks instance = null;
    public Button backButton;
    public Text GameIDText;
    public Text DifficultyText;
    public Text LevelText;

    // public string GameID;
    // public string Difficulty;
    // public string Level;

    public static CheckAssignedTasks GetCheckAssignedTasks()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<CheckAssignedTasks>();
        }
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void BackToTeacherMenu()
    {
        scene.ToTeacherSelectTaskScreen();
    }

    public void SetAttributes(string gameID, Text GameID, string difficulty, Text Difficulty, string level, Text Level)
    {
        // GameID = gameID;
        GameIDText = GameID;
        // Difficulty = difficulty;
        DifficultyText = Difficulty;
        // Level = level;
        LevelText = Level;

        GameIDText.text = gameID;
        DifficultyText.text = difficulty;
        LevelText.text = level;
    }
}
