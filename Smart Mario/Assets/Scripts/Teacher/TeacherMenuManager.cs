using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller for the teacher menu scene
/// </summary>
public class TeacherMenuManager : MonoBehaviour
{
    //Singleton
    private static TeacherMenuManager instance = null;

    private SceneController scene;

    public Text teacherIDMessage;
    public Text welcomeMessage;
    public Button studentPerformanceButton;
    public Button assignTaskButton;
    public Button checkAssignedTasksButton;
    public Button logOutButton;

    private static string teacherID;
    private APICall apiCall;

    /// <summary>
    ///  Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
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
        teacherIDMessage.text = "ID: " + teacherID;
        welcomeMessage.text = "Welcome " + PlayerPrefs.GetString("username") + "!";
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    /// <summary>
    /// Select Student Performance Button action; transits to select student performance screen
    /// </summary>
    public void SelectStudentPerformanceScreen()
    {
        StartCoroutine(apiCall.AllStudentResultGetRequest(teacherID, true));
    }

    /// <summary>
    /// Assign task button action; transits to assign task screen
    /// </summary>
    public void AssignTasksScreen()
    {
        scene.ToAssignTasksScreen();
    }

    /// <summary>
    /// Selet task screen button, transits to select task screen
    /// </summary>
    public void SelectTaskScreen()
    {
        StartCoroutine(apiCall.AllStudentResultGetRequest(teacherID, false));
    }

    /// <summary>
    /// Log out button action; transits to login screen
    /// </summary>
    public void LogOut()
    {
        scene.ToLogin();
    }
}
