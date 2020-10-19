using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTaskManager : MonoBehaviour
{
    //Singleton
    private static SelectTaskManager instance = null;
    
    private SceneController scene;
    private TeacherMenu teacherMenu;

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
        teacherMenu = TeacherMenu.GetTeacherMenu();
        teacherId = teacherMenu.GetTeacherId();
        SetDefaultValues();

        DisplayListManager displayListManager = DisplayListManager.GetDisplayListManager();
        displayResultsList = displayListManager.GetDisplayResultsList();
        tasksList = ParseList(displayResultsList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetDefaultValues()
    {
        minigameId = "1";
        difficulty = "easy";
        level = "1"; 
    }

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


    public string GetMiniGameID() { return minigameId; }

    public string GetDifficulty() { return difficulty; }

    public string GetLevel() { return level; }
}
