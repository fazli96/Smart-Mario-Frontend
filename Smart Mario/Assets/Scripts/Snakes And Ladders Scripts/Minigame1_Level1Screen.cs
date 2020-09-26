using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Minigame1_Level1Screen : MonoBehaviour
{

    public GameObject pausePanel;
    private bool isPaused;
    private SceneController scene;
    
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        pausePanel.SetActive(false);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                isPaused = true;
                pausePanel.SetActive(true);
            }
            else
            {
                isPaused = false;
                pausePanel.SetActive(false);
            }
        }
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
        scene.ToMinigame1Level1();
    }

    public void NextLevel()
    {
        //
    }

}
