using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CheckAssignedTasks : MonoBehaviour
{
    private SceneController scene;

    public Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void BackToTeacherMenu()
    {
        scene.ToTeacherMenu();
    }
}
