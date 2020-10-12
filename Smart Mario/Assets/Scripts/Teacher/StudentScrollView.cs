using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentScrollView : MonoBehaviour
{
    private SceneController scene;

    [SerializeField]
    private GameObject Button_Template;
    [SerializeField]
    private GridLayoutGroup gridGroup;
    private List<string> NameList = new List<string>();
    public Text msg;


    // Start is called before the first frame update
    void Start()
    {
        DisplayMessage("");

        scene = SceneController.GetSceneController();

        // NameList.Add("Alan");
        // NameList.Add("Amy");
        // NameList.Add("Brian");
        // NameList.Add("Carrie");
        // NameList.Add("David");
        // NameList.Add("Joe");
        // NameList.Add("Jason");
        // NameList.Add("Michelle");
        // NameList.Add("Stephanie");
        // NameList.Add("Zoe");
        // NameList.Add("Alan2");
        // NameList.Add("Amy2");
        // NameList.Add("Brian2");
        // NameList.Add("Carrie2");
        // NameList.Add("David2");
        // NameList.Add("Joe2");
        // NameList.Add("Jason2");
        // NameList.Add("Michelle2");
        // NameList.Add("Stephanie2");
        // NameList.Add("Zoe2");
        
        GenerateStudentButtons();

        if (NameList.Count == 0)
        {
            DisplayMessage("There are no students to display.");
        }

    }

    private void GenerateStudentButtons()
    {
        foreach(string str in NameList)
        {
            GameObject newButton = Instantiate(Button_Template) as GameObject;
            newButton.SetActive(true);
            StudentButton SB = newButton.GetComponent<StudentButton>();
            SB.SetName(str);
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

    public void ButtonClicked(string str)
    {
        scene.ToStudentPerformance();
    }

}