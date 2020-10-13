using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTaskManager : MonoBehaviour
{
    private SceneController scene;

    public Button nextButton;
    public Button backButton;
    public Text errorMessage;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        DisplayErrorMessage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayErrorMessage()
    {   
        errorMessage.text = "Task has not been assigned yet!";
    }

    public void Back()
    {
        scene.ToTeacherMenu();
    }

    public void Next()
    {
        // load first
        scene.ToViewAssignedTasksScreen();
    }
}
