using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Minigame1Level1_1Screen : MonoBehaviour
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

    public void ToWorld1()
    {
        scene.PlayWorld1();
    }

    public void ToLevelSelection()
    {
        //
    }

    public void RestartLevel()
    {
        scene.ToMinigame1();
    }

    public void NextLevel()
    {
        //
    }

}
