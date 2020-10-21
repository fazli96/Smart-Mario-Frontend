using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public static StudentScrollViewController GetStudentScrollViewController()
    {
        if (instance == null)
        {
            instance = new StudentScrollViewController();
        }
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        DisplayMessage("");

        scene = SceneController.GetSceneController();

        DisplayListManager displayListManager = DisplayListManager.GetDisplayListManager();
        displayResultsList = displayListManager.GetDisplayResultsList();
        nameList = ParseList(displayResultsList);

        if (nameList.Count != 0)
        {
            GenerateStudentButtons();
        }
        else
        {
            DisplayMessage("There are no students to display.");
        }
    }

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

    public void ButtonClicked()
    {
        scene.ToStatistics();
    }

}