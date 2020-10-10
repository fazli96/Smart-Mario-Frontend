using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignedTasksScrollView : MonoBehaviour
{
    private SceneController scene;

    [SerializeField]
    private GameObject Button_Template;
    [SerializeField]
    private GridLayoutGroup gridGroup;
    private bool TaskisAssigned = false;
    private List<StudentTaskCell> StudentList = new List<StudentTaskCell>();
    public Text msg;
    
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();

        // Get Logic of whether task had been assigned or not.
        TaskisAssigned = false;

        if (TaskisAssigned)
        {
            DisplayMessage("");

            // StudentList.Add(new StudentTaskCell("Alan", true));
            // StudentList.Add(new StudentTaskCell("Amy"));
            // StudentList.Add(new StudentTaskCell("Brian"));
            // StudentList.Add(new StudentTaskCell("Carrie"));
            // StudentList.Add(new StudentTaskCell("David", true));
            // StudentList.Add(new StudentTaskCell("Joe"));
            // StudentList.Add(new StudentTaskCell("Jason"));
            // StudentList.Add(new StudentTaskCell("Michelle", true));
            // StudentList.Add(new StudentTaskCell("Stephanie"));
            // StudentList.Add(new StudentTaskCell("Zoe"));
            // StudentList.Add(new StudentTaskCell("Alan2", true));
            // StudentList.Add(new StudentTaskCell("Amy2"));
            // StudentList.Add(new StudentTaskCell("Brian2"));
            // StudentList.Add(new StudentTaskCell("Carrie2", true));
            // StudentList.Add(new StudentTaskCell("David2"));
            // StudentList.Add(new StudentTaskCell("Joe2"));
            // StudentList.Add(new StudentTaskCell("Jason2"));
            // StudentList.Add(new StudentTaskCell("Michelle2", true));
            // StudentList.Add(new StudentTaskCell("Stephanie2"));
            // StudentList.Add(new StudentTaskCell("Zoe2"));

            GenerateStudentCells();
        }
        
        else
        {
            DisplayMessage("Task has not been assigned yet!");
        }
        
    }

    private void GenerateStudentCells()
    {
        foreach(StudentTaskCell student in StudentList)
        {
            GameObject newButton = Instantiate(Button_Template) as GameObject;
            newButton.SetActive(true);
            StudentTaskCompletionCell cell = newButton.GetComponent<StudentTaskCompletionCell>();
            cell.SetName(student.GetName());
            cell.SetCompletionStatus(student.GetCompletionStatus());
            newButton.transform.SetParent(Button_Template.transform.parent);
        }
    }
    
    public void DisplayMessage(string str)
    {
        msg.text = str;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
