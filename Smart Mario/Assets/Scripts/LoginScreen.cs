using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScreen : MonoBehaviour
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

    public void validateLogin()
    {
        //check against database
    }

    public void ReturnToStartMenu()
    {
        scene.ToStartMenu();
    }
}
