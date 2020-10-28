using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeacherMenuManager : MonoBehaviour
{
    //Singleton
    private static TeacherMenuManager instance = null;

    private SceneController scene;

    public Text welcomeMessage;
    public Button studentPerformanceButton;
    public Button assignTaskButton;
    public Button checkAssignedTasksButton;
    public Button logOutButton;

    private static string teacherID;
    private APICall apiCall;

    public static TeacherMenuManager GetTeacherMenuManager()
    {
        if (instance == null)
        {
            instance = new TeacherMenuManager();
        }
        return instance;
    }


    // Start is called before the first frame update
    void Start()
    {
        apiCall = APICall.getAPICall();
        scene = SceneController.GetSceneController();
        teacherID = PlayerPrefs.GetString("teacherId");
        welcomeMessage.text = "Welcome " + PlayerPrefs.GetString("username") + "!";
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public void SelectStudentPerformanceScreen()
    {
        StartCoroutine(apiCall.AllStudentResultGetRequest(teacherID, true));
    }

    public void AssignTasksScreen()
    {
        scene.ToAssignTasksScreen();
    }

    public void SelectTaskScreen()
    {
        StartCoroutine(apiCall.AllStudentResultGetRequest(teacherID, false));
    }

    public void LogOut()
    {
        scene.ToLogin();
    }
}
