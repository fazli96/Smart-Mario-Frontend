using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : MonoBehaviour
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
}
