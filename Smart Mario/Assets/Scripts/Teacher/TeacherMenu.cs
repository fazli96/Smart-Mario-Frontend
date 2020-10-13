using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeacherMenu : MonoBehaviour
{
    private SceneController scene;

    public Button studentPerformanceButton;
    public Button assignTaskButton;
    public Button checkAssignedTasksButton;
    public Button logOutButton;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectStudentPerformanceScreen()
    {
        scene.ToSelectStudentPerformance();
    }

    public void AssignTasksScreen()
    {
        scene.ToAssignTasksScreen();
    }

    public void SelectTaskScreen()
    {
        scene.ToTeacherSelectTaskScreen();
    }

    public void LogOut()
    {
        scene.ToLogin();
    }
}
