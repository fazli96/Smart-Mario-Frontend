using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
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

    public void CreateAccount()
    {
        scene.ToCreateAccount();
    }

    public void Login()
    {
        scene.ToLogin();
    }

    public void Quit()
    {
        scene.ToQuit();
    }
}
