using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTaskManager : MonoBehaviour
{
    //Singleton
    private static SelectTaskManager instance = null;
    
    private SceneController scene;

    public static string gameName;
    public static string difficulty;
    public static string level;

    public Button nextButton;
    public Button backButton;
    public Text errorMessage;

    // string teacherId;
    // string minigameId;
    // string difficulty;
    // string level;

    public static SelectTaskManager GetSelectTaskManager()
    {
        if (instance == null)
        {
            instance = new SelectTaskManager();
        }
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        SetDefaultValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetDefaultValues()
    {
        gameName = "World 1 Stranded";
        difficulty = "Easy";
        level = "Level 1"; 
    }

    public void DisplayErrorMessage()
    {   
        errorMessage.text = "Task has not been assigned yet!";
    }

    public void Back()
    {
        scene.ToTeacherMenu();
    }

    public void Next()
    {
        APICall api = APICall.getAPICall();
        StartCoroutine(api.SpecificTaskResult("2", "2", "hard", "1"));
    }

    public string GetGameID() { return gameName; }

    public string GetDifficulty() { return difficulty; }

    public string GetLevel() { return level; }
}
