using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeacherMenu : MonoBehaviour
{
    //Singleton
    private static TeacherMenu instance = null;

    private SceneController scene;

    public Button studentPerformanceButton;
    public Button assignTaskButton;
    public Button checkAssignedTasksButton;
    public Button logOutButton;

    private static string teacherID;
    private APICall apiCall;

    public static TeacherMenu GetTeacherMenu()
    {
        if (instance == null)
        {
            instance = new TeacherMenu();
        }
        return instance;
    }


    // Start is called before the first frame update
    void Start()
    {
        apiCall = APICall.getAPICall();
        scene = SceneController.GetSceneController();
        // teacherID = .GetTeacherId();
        teacherID = "1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectStudentPerformanceScreen()
    {
        StartCoroutine(apiCall.AllStudentResultGetRequest(teacherID, 0));
    }

    public void AssignTasksScreen()
    {
        scene.ToAssignTasksScreen();
    }

    public void SelectTaskScreen()
    {
        StartCoroutine(apiCall.AllStudentResultGetRequest(teacherID, 1));
    }

    public void LogOut()
    {
        scene.ToLogin();
    }

    public string GetTeacherId()
    {
        return teacherID;
    }
}
