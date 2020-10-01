using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherMenu : MonoBehaviour
{
    private SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StudentPerformanceScreen()
    {
        scene.ToStudentPerformance();
    }

    public void AssignTasksScreen()
    {
        scene.ToAssignTasksScreen();
    }

    public void CheckAssignedTasksScreen()
    {
        scene.ToCheckAssignedTasksScreen();
    }

    public void LogOut()
    {
        scene.ToLogin();
    }
}
