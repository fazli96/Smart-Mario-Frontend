using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  Boundary that connects to Unity World Selection Scene UI objects and triggers function calls on events
/// </summary>
public class WorldSelectionManager : MonoBehaviour
{
    private SceneController scene;
    // Start is called before the first frame update
    /// <summary>
    /// Get instance of SceneController once World Selection starts
    /// </summary>
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Changes scene to World 1 and sets player preference to reflect World 1 
    /// </summary>
    public void SelectWorld1()
    {
        PlayerPrefs.SetInt("World", 1);
        scene.PlayWorld1();
    }
    /// <summary>
    /// Changes scene to World 2 and sets player preference to reflect World 2 
    /// </summary>
    public void SelectWorld2()
    {
        PlayerPrefs.SetInt("World", 2);
        scene.PlayWorld2();
    }
    /// <summary>
    /// Changes scene to Main Menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        scene.ToMainMenu();
    }
}
