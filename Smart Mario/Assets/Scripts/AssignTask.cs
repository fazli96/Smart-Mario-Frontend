using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignTask : MonoBehaviour
{
    private SceneController scene;

    public Button confirmButton;
    public Button cancelButton;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Confirm()
    {
        
    }

    public void Cancel()
    {
        scene.ToTeacherMenu();
    }
}
