using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour
{
    private static MainMenuScreen instance = null;
    private SceneController scene;
    public Text msg;

    public static MainMenuScreen GetMainMenuScreen()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<MainMenuScreen>();
        }
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
      //  GameObject obj = GameObject.Find("Title");
   //     msg = obj.GetComponent<Text>();
    //    UnityEngine.Debug.Log("Welcome " + PlayerPrefs.GetString("username") + "!");
        msg.text = "Welcome " + PlayerPrefs.GetString("username") + "!";
    }
    
    // Update is called once per frame
    void Update()
    {
    }

    public void WorldSelectionScreen()
    {
        scene.ToWorldSelection();
    }

    public void MultiplayerScreen()
    {
        
    }

    public void CustomizeCharacterScreen()
    {
        scene.ToCustomizeCharacter();
    }

    public void LogOut()
    {
        scene.ToLogin();
    }

  /*  public void DisplayMessage(string message)
    {
        msg = instance.GetComponent<Text>();
        msg.text = message;
    }*/
}
