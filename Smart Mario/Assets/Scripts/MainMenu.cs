using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.getSceneController();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WorldSelection()
    {
        scene.ToWorldSelection();
    }

    public void ReturnToStartMenu()
    {
        scene.ToStartMenu();
    }
}
