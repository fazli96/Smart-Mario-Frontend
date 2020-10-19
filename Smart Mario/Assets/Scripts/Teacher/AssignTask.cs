using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignTask : MonoBehaviour
{
    //Singleton
    private static AssignTask instance = null;

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

    public static AssignTask GetAssignTask()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<AssignTask>();
        }
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        waitMessage.text = "";
        scene = SceneController.GetSceneController();
        TeacherMenu teacherMenu = TeacherMenu.GetTeacherMenu();
        teacherId  = teacherMenu.GetTeacherId();
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

    public void ConfirmAndDisplayMessage()
    {
        successMessage.text = "";
        errorMessage.text = "";
        waitMessage.text = "Authenticating...\nPlease hold on.";
        var currentSelection = (minigameId, difficulty, level);
        APICall apiCall = APICall.getAPICall();
        StartCoroutine(apiCall.AssignTaskPutRequest(teacherId, minigameId, difficulty, level));
    }

    public void setRefresh()
    {
        refresh = true;
    }

    public void setSuccessStatus(bool status)
    {
        success = status;
    }

    public void Cancel()
    {
        scene.ToTeacherMenu();
    }

}
