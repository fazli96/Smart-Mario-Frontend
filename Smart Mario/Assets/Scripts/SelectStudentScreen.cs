using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStudentScreen : MonoBehaviour
{
    private SceneController scene;

    public Button exportCSVButton;
    public Button backToTeacherMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExportCSV()
    {
        
    }

    public void BackToTeacherMenu()
    {
        scene.ToTeacherMenu();
    }
}
