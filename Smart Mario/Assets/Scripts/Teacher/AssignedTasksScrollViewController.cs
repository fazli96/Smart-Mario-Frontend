using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Controller class for teh display of the assigned tasks scroll view
/// </summary>
public class AssignedTasksScrollViewController : MonoBehaviour
{
    private SceneController scene;

    public GameObject Button_Template;
    public GridLayoutGroup gridGroup;

    public AssignedTasksManager assignedTasksManager = AssignedTasksManager.GetAssignedTasksManager();

    private static List<StudentTaskCell> studentList = new List<StudentTaskCell>();
    public Text msg;


    /// <summary>
    /// Gets the stored list of students and generates a cell for each student along with their completion status
    /// </summary>
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

    /// <summary>
    /// Generates a cell for each student
    /// </summary>
    private void GenerateStudentCells()
    {
        foreach(StudentTaskCell student in studentList)
        {
            GameObject newButton = Instantiate(Button_Template) as GameObject;
            newButton.SetActive(true);
            StudentTaskCompletionCell cell = newButton.GetComponent<StudentTaskCompletionCell>();
            cell.SetCell(student.GetName(), student.GetCompletionStatus());
            newButton.transform.SetParent(Button_Template.transform.parent, false);
        }
    }
    
    /// <summary>
    /// To display a string message
    /// </summary>
    /// <param name="str"></param>
    public void DisplayMessage(string str)
    {
        msg.text = str;
    }

    // Update is called once per fram
    void Update()
    {
        
    }
}
