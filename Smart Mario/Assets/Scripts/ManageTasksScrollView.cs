using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageTasksScrollView : MonoBehaviour
{
    //Singleton
    private static ManageTasksScrollView instance = null;

    private SceneController scene;

    [SerializeField]
    private GameObject Button_Template;
    [SerializeField]
    private GridLayoutGroup gridGroup;

    private List<DisplayResults> displayResultsList;
    private List<TaskCompletionCell> TasksList = new List<TaskCompletionCell>();
    public Text noTasksMessage;

    public static ManageTasksScrollView GetManageTasksScrollView()
    {
        if (instance == null)
        {
            instance = new ManageTasksScrollView();
        }
        return instance;
    }


    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        
        DisplayListManager displayListManager = DisplayListManager.GetDisplayListManager();
        displayResultsList = displayListManager.GetDisplayResultsList();
        
        TasksList = ParseList(displayResultsList);
        GenerateTaskCells(TasksList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<TaskCompletionCell> ParseList(List<DisplayResults> ls)
    {
        List<TaskCompletionCell> newList = new List<TaskCompletionCell>();
        foreach (DisplayResults item in ls)
        {
            newList.Add(new TaskCompletionCell(item.minigameId, item.difficulty, item.level, item.completed));
        }
        return newList;
    }

    private void GenerateTaskCells(List<TaskCompletionCell> ls)
    {
        foreach(TaskCompletionCell task in ls)
        {
            GameObject newButton = Instantiate(Button_Template) as GameObject;
            newButton.SetActive(true);
            TaskCell cell = newButton.GetComponent<TaskCell>();
            cell.SetCell(task.GetGameID(), task.GetDifficulty(), task.GetLevel(), task.GetCompletionStatus());
            newButton.transform.SetParent(Button_Template.transform.parent);
        }
    }

    public void DisplayNoTasksMessage()
    {
        noTasksMessage.text = "You have no assigned tasks!";
    }
}
