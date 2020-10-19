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

    public GameObject Button_Template;
    public GridLayoutGroup gridGroup;

    private static List<DisplayResults> displayResultsList;
    private static List<TaskCompletionCell> tasksList = new List<TaskCompletionCell>();
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

    private List<TaskCompletionCell> ParseList(List<DisplayResults> ls)
    {
        List<TaskCompletionCell> newList = new List<TaskCompletionCell>();
        foreach (DisplayResults item in ls)
        {
            Translator translator  = Translator.GetTranslator();
            string gameName = translator.GameIDToName(item.minigameId.ToString());
            string level = "Level " + item.level;
            newList.Add(new TaskCompletionCell(gameName, item.difficulty, level, item.completed));
        }
        return newList;
    }

    private void GenerateTaskCells()
    {
        foreach(TaskCompletionCell task in tasksList)
        {
            GameObject newButton = Instantiate(Button_Template) as GameObject;
            newButton.SetActive(true);
            TaskCell cell = newButton.GetComponent<TaskCell>();
            cell.SetCell(task.GetGameName(), task.GetDifficulty(), task.GetLevel(), task.GetCompletionStatus());
            newButton.transform.SetParent(Button_Template.transform.parent);
        }
    }

    public void DisplayNoTasksMessage()
    {
        noTasksMessage.text = "You have no assigned tasks!";
    }
}
