using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AssignedTasksScrollViewController : MonoBehaviour
{
    private SceneController scene;

    public GameObject Button_Template;
    public GridLayoutGroup gridGroup;

    public AssignedTasksManager assignedTasksManager = AssignedTasksManager.GetAssignedTasksManager();

    private static List<StudentTaskCell> studentList = new List<StudentTaskCell>();
    public Text msg;


    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();

        studentList = assignedTasksManager.GetStudentList();
        if (studentList != null)
        {
            if (studentList.Count == 0)
            {
                DisplayMessage("There are no students to display.");
            }
            else
            {
                GenerateStudentCells();
            }
        }
        else
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
