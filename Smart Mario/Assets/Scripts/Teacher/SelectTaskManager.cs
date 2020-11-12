using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Select a task to view for Teacher to check student's completion status or whether the task has been assigned
/// </summary>
public class SelectTaskManager : MonoBehaviour
{
    //Singleton
    private static SelectTaskManager instance = null;
    
    private SceneController scene;
    private TeacherMenuManager teacherMenuManager;

    public Dropdown minigameDropdown;
    public Dropdown difficultyDropdown;
    public Dropdown levelDropdown;

    public Button nextButton;
    public Button backButton;
    public Text errorMessage;

    private static List<DisplayResults> displayResultsList;
    private static List<(string, string, string)> tasksList;
    private static string teacherId;
    private static string minigameId;
    private static string difficulty;
    private static string level;

    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static SelectTaskManager GetSelectTaskManager()
    {
        if (instance == null)
        {
            instance = new SelectTaskManager();
        }
        return instance;
    }

    /// <summary>
    /// Gets the list of tasks from students and extracts the tasks assigned.
    /// </summary>
    void Start()
    {
        scene = SceneController.GetSceneController();
        teacherMenuManager = TeacherMenuManager.GetTeacherMenuManager();
        teacherId = PlayerPrefs.GetString("teacherId");
        SetDefaultValues();

        DisplayListManager displayListManager = DisplayListManager.GetDisplayListManager();
        displayResultsList = displayListManager.GetDisplayResultsList();
        tasksList = ParseList(displayResultsList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Sets the default values for the dropdown list
    /// </summary>
    private void SetDefaultValues()
    {
        minigameId = "1";
        difficulty = "easy";
        level = "1"; 
    }

    /// <summary>
    /// Changes the seelcted value when user selects an option from the dropdown menu
    /// </summary>
    public void OnDropdownValueChange()
    {
        minigameId = (minigameDropdown.value + 1).ToString();
        level = (levelDropdown.value + 1).ToString();
        switch (difficultyDropdown.value)
        {
            case 0:
                difficulty = "easy";
                break;
            case 1:
                difficulty = "medium";
                break;
            case 2:
                difficulty = "hard";
                break;
            default:
                difficulty = "easy";
                break;
        }
    }

    /// <summary>
    /// Converts the list to what we need
    /// </summary>
    /// <param name="ls"></param>
    /// <returns></returns>
    private List<(string, string, string)> ParseList(List<DisplayResults> ls)
    {
        List<(string, string, string)> newList = new List<(string, string, string)>();
        foreach (DisplayResults item in ls)
        {
            var newTask = (item.minigameId.ToString(), item.difficulty.ToString(), item.level.ToString());
            if (!newList.Contains(newTask))
            {
                newList.Add(newTask);
            }
        }
        return newList;
    }

    /// <summary>
    /// Displays an error message that the task had not been assigned yet
    /// </summary>
    public void DisplayErrorMessage()
    {   
        errorMessage.text = "Task has not been assigned yet!";
    }

    /// <summary>
    /// Back button action
    /// </summary>
    public void Back()
    {
        scene.ToTeacherMenu();
    }

    /// <summary>
    /// Next button action
    /// </summary>
    public void Next()
    {
        var currentSelection = (minigameId, difficulty, level);
        UnityEngine.Debug.Log(currentSelection);
        bool taskIsAssigned = tasksList.Contains(currentSelection);
        if (taskIsAssigned)
        {
            APICall api = APICall.getAPICall();
            StartCoroutine(api.SpecificTaskResult(teacherId, minigameId, difficulty, level));
        }
        else
        {
            DisplayErrorMessage();
        }
    }


    /// <summary>
    /// Get the minigame id of the user's current selection
    /// </summary>
    /// <returns></returns>
    public string GetMiniGameID() { return minigameId; }

    /// <summary>
    /// Get the difficulty of the user's current selection
    /// </summary>
    /// <returns></returns>
    public string GetDifficulty() { return difficulty; }

    /// <summary>
    /// Get the level of the user's current selection
    /// </summary>
    /// <returns></returns>
    public string GetLevel() { return level; }
}
