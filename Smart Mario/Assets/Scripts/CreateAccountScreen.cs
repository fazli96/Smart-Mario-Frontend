using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAccountScreen : MonoBehaviour
{
    private SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void validateKey()
    {
        //checks if key is valid before proceeding with username and password storage
    }

    public void storeUsername()
    {
        //checks if email contains @
        //if not display error message, else
        //calls truncateUsername
        //stores username in database
    }

    public void storePassword()
    {
        //stores password in database
    }

    public void ReturnToStartMenu()
    {
        scene.ToStartMenu();
    }

    private string truncateUsername(string email)
    {
        //truncate username from email address
        int pos;
        pos = email.IndexOf("@");
        return email.Substring(0, pos + 1);
    }
}
