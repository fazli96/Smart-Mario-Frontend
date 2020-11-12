using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for the scroll view selection of students to view their performance
/// </summary>
public class StudentScrollViewController : MonoBehaviour
{
    //Singleton
    private static StudentScrollViewController instance = null;

    private SceneController scene;

    [SerializeField]
    private GameObject Button_Template;
    [SerializeField]
    private GridLayoutGroup gridGroup;
    
    private static List<DisplayResults> displayResultsList;
    private static List<(string, string)> nameList;
    public Text msg;

    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static StudentScrollViewController GetStudentScrollViewController()
    {
        if (instance == null)
        {
            instance = new StudentScrollViewController();
        }
        return instance;
    }

    /// <summary>
    /// Gets list of students then generate buttons for each student
    /// </summary>
    void Start()
    {
        DisplayMessage("");

        scene = SceneController.GetSceneController();

        DisplayListManager displayListManager = DisplayListManager.GetDisplayListManager();
        displayResultsList = displayListManager.GetDisplayResultsList();

        if (displayResultsList != null)
        {
            if (displayResultsList.Count != 0)
            {
                nameList = ParseList(displayResultsList);
                GenerateStudentButtons();
            }
            else
            {
                DisplayMessage("There are no students to display.");
            }
        }
        else
        {
            DisplayMessage("There are no students to display.");
        }

    }

    /// <summary>
    /// Converts the list items to what the buttons need
    /// </summary>
    /// <param name="ls"></param>
    /// <returns></returns>
    private List<(string, string)> ParseList(List<DisplayResults> ls)
    {
        List<(string, string)> newList = new List<(string, string)>();
        foreach (DisplayResults item in ls)
        {
            string name = item.studentName;
            var student = (name, item.studentId.ToString());
            if (!newList.Contains(student))
            {
                newList.Add(student);
            }
        }
        return newList;
    }

    /// <summary>
    /// Creates a button for each student
    /// </summary>
    private void GenerateStudentButtons()
    {
        foreach(var student in nameList)
        {
            string name = student.Item1;
            string id = student.Item2;
            GameObject newButton = Instantiate(Button_Template) as GameObject;
            newButton.SetActive(true);
            StudentButton SB = newButton.GetComponent<StudentButton>();
            SB.SetAttributes(name, id);
            newButton.transform.SetParent(Button_Template.transform.parent, false);
        }
    }

    /// <summary>
    /// Function to diaplay an error message
    /// </summary>
    /// <param name="str"></param>
    public void DisplayMessage(string str)
    {
        msg.text = str;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Action for when a student button is clicked
    /// </summary>
    public void ButtonClicked()
    {
        scene.ToStatistics();
    }

}