using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignTask : MonoBehaviour
{
    private SceneController scene;

    public Button confirmButton;
    public Button cancelButton;
    public Text successMessage;
    public Text errorMessage;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfirmAndDisplayMessage()
    {
        // To replace
        bool success = false;
        
        successMessage.text = success ? "Task successfully assigned to all students!" : "" ;
        errorMessage.text = success ? "" : "Task is not assigned, as it was already assigned before!";
    }

    public void Cancel()
    {
        scene.ToTeacherMenu();
    }
    
}
