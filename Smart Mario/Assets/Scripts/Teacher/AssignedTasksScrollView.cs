using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AssignedTasksScrollView : MonoBehaviour
{
    //Singleton
    private static AssignedTasksScrollView instance = null;
 
    private SceneController scene;

    public GameObject Button_Template;
    public GridLayoutGroup gridGroup;

    public AssignedTasksManager assignedTasksManager = AssignedTasksManager.GetAssignedTasksManager();


    private static List<StudentTaskCell> studentList = new List<StudentTaskCell>();
    public Text msg;

    private bool taskisAssigned = false;
    
    public static AssignedTasksScrollView GetAssignedTasksScrollView()
    {
        if (instance == null)
        {
            instance = new AssignedTasksScrollView();
        }
        return instance;
    }


    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();

        DisplayMessage("");

        studentList = assignedTasksManager.GetStudentList();
        GenerateStudentCells();

        if (studentList.Count == 0)
        {
            DisplayMessage("There are no students to display.");
        }
    }

    private void GenerateStudentCells()
    {
        foreach(StudentTaskCell student in studentList)
        {
            GameObject newButton = Instantiate(Button_Template) as GameObject;
            newButton.SetActive(true);
            StudentTaskCompletionCell cell = newButton.GetComponent<StudentTaskCompletionCell>();
            cell.SetCell(student.GetName(), student.GetCompletionStatus());
            newButton.transform.SetParent(Button_Template.transform.parent);
        }
    }
    
    public void DisplayMessage(string str)
    {
        msg.text = str;
    }

    // Update is called once per fram
    void Update()
    {
        
    }
}
