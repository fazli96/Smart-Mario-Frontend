using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Controller of the manage tasks scroll view. It dynamically displays the tasks.
/// </summary>
public class ManageTasksScrollViewManager : MonoBehaviour
{
    private SceneController scene;

    public GameObject Button_Template;
    public GridLayoutGroup gridGroup;

    private static List<DisplayResults> displayResultsList;
    private static List<TaskCompletionCell> tasksList = new List<TaskCompletionCell>();
    public Text noTasksMessage;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
                
        DisplayListManager displayListManager = DisplayListManager.GetDisplayListManager();
        displayResultsList = displayListManager.GetDisplayResultsList();
        tasksList = ParseList(displayResultsList);

        if (tasksList.Count != 0)
        {
            GenerateTaskCells();
        }
        else
        {
            DisplayNoTasksMessage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Converts the list of items from DisplayList Manager to the list of attributes it needs
    /// </summary>
    /// <param name="ls"></param>
    /// <returns></returns>
    private List<TaskCompletionCell> ParseList(List<DisplayResults> ls)
    {
        List<TaskCompletionCell> newList = new List<TaskCompletionCell>();
        Translator translator  = Translator.GetTranslator();
        foreach (DisplayResults item in ls)
        {
            string gameName = translator.GameIDToName(item.minigameId.ToString());
            string level = "Level " + item.level;
            newList.Add(new TaskCompletionCell(gameName, item.difficulty, level, item.createdAt, item.completed));
        }
        return newList;
    }

    /// <summary>
    /// Create the cells to be displayed
    /// </summary>
    private void GenerateTaskCells()
    {
        foreach(TaskCompletionCell task in tasksList)
        {
            GameObject newButton = Instantiate(Button_Template) as GameObject;
            newButton.SetActive(true);
            TaskCell cell = newButton.GetComponent<TaskCell>();
            cell.SetCell(task.GetGameName(), task.GetDifficulty(), task.GetLevel(), task.GetCreatedAt(), task.GetCompletionStatus());
            newButton.transform.SetParent(Button_Template.transform.parent, false);
        }
    }

    /// <summary>
    /// Displays error message if there are no tasks
    /// </summary>
    public void DisplayNoTasksMessage()
    {
        noTasksMessage.text = "You have no assigned tasks!";
    }
}
