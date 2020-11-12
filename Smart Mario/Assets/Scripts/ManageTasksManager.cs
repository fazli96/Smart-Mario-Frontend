using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller class for the Manage Task Scene for student
/// </summary>
public class ManageTasksManager : MonoBehaviour
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


    /// <summary>
    /// Back button action
    /// </summary>
    public void Back()
    {
        scene.ToMainMenu();
    }
}
