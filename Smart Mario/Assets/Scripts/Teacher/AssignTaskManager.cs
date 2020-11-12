using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller responsible for allowing the teacher to assign a task for the students
/// </summary>
public class AssignTaskManager : MonoBehaviour
{
    //Singleton
    private static AssignTaskManager instance = null;

    private SceneController scene;

    public Dropdown minigameDropdown;
    public Dropdown difficultyDropdown;
    public Dropdown levelDropdown;

    public Button confirmButton;
    public Button cancelButton;
    public Text waitMessage;
    public Text successMessage;
    public Text errorMessage;

    private static string teacherId;
    private static string minigameId;
    private static string difficulty;
    private static string level;
    private static bool refresh;
    private static bool success;

    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static AssignTaskManager GetAssignTaskManager()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<AssignTaskManager>();
        }
        return instance;
    }

    /// <summary>
    /// Set the default values for the dropdown list.
    /// If teacher attempts to assign a task, the scene is refreshed. 
    /// If refreshed, then it will check the success status of assigning the task and display the outcome.
    /// </summary>
    void Start()
    {
        waitMessage.text = "";
        scene = SceneController.GetSceneController();
        TeacherMenuManager teacherMenuManager = TeacherMenuManager.GetTeacherMenuManager();
        teacherId = PlayerPrefs.GetString("teacherId");
        SetDefaultValues();

        if (refresh)
        {
            successMessage.text = success ? "Task successfully assigned to all students!" : "" ;
            errorMessage.text = success ? "" : "Task is not assigned, as it was already assigned before!";
        }

        refresh = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Set default values for the dropdown menus
    /// </summary>
    private void SetDefaultValues()
    {
        minigameId = "1";
        difficulty = "easy";
        level = "1"; 
    }

    /// <summary>
    /// Changes current value when user selects an option from the dropdown menu
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
    /// Confirm button action
    /// Starts coroutine of assigning the task
    /// </summary>
    public void ConfirmAndDisplayMessage()
    {
        successMessage.text = "";
        errorMessage.text = "";
        waitMessage.text = "Authenticating...\nPlease hold on.";
        var currentSelection = (minigameId, difficulty, level);
        APICall apiCall = APICall.getAPICall();
        StartCoroutine(apiCall.AssignTaskPutRequest(teacherId, minigameId, difficulty, level));
    }

    /// <summary>
    /// For coroutine to set the refresh flag to True before loading the scene again
    /// </summary>
    public void setRefresh()
    {
        refresh = true;
    }

    /// <summary>
    /// Coroutine sets the success status of assigning the task
    /// </summary>
    /// <param name="status"></param>
    public void setSuccessStatus(bool status)
    {
        success = status;
    }

    /// <summary>
    /// Cancel button action
    /// </summary>
    public void Cancel()
    {
        scene.ToTeacherMenu();
    }

}
