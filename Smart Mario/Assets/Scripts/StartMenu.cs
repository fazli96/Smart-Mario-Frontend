using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Boundary that connects to Unity Start Scene UI objects and triggers function calls on events
/// </summary>
public class StartMenu : MonoBehaviour
{
    private SceneController scene;
    /// <summary>
    /// Get instances of SceneController once StartMenu starts
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    // Update is called once per frames
    void Update()
    {
        
    }
    /// <summary>
    /// Changes scene to CreateAccount
    /// </summary>
    public void CreateAccount()
    {
        scene.ToCreateAccount();
    }
    /// <summary>
    /// Changes scene to Login 
    /// </summary>
    public void Login()
    {
        scene.ToLogin();
    }
    /// <summary>
    /// Application terminates
    /// </summary>
    public void Quit()
    {
        scene.ToQuit();
    }
}
