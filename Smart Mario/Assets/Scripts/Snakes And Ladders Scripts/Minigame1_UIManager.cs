using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Minigame1_UIManager : MonoBehaviour
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

    public void ToWorld()
    {
        scene.PlayWorld1();
    }

    public void ToLevelSelection()
    {
        scene.ToLevelSelection();
    }

    public void RestartLevel()
    {
        switch (PlayerPrefs.GetInt("World1Minigame1Level", 1))
        {
            case 1:
                scene.ToWorld1Minigame1Level1();
                break;
            case 2:
                scene.ToWorld1Minigame1Level2();
                break;
            case 3:
                scene.ToWorld1Minigame1Level3();
                break;
            case 4:
                scene.ToWorld1Minigame1Level4();
                break;
            case 5:
                scene.ToWorld1Minigame1Level5();
                break;
            default:
                break;
        }
    }

    public void NextLevel()
    {
        switch (PlayerPrefs.GetInt("World1Minigame1Level", 1))
        {
            
            case 1:
                PlayerPrefs.SetInt("World1Minigame1Level", 2);
                scene.ToWorld1Minigame1Level2();
                break;
            case 2:
                PlayerPrefs.SetInt("World1Minigame1Level", 3);
                scene.ToWorld1Minigame1Level3();
                break;
            case 3:
                PlayerPrefs.SetInt("World1Minigame1Level", 4);
                scene.ToWorld1Minigame1Level4();
                break;
            case 4:
                PlayerPrefs.SetInt("World1Minigame1Level", 5);
                scene.ToWorld1Minigame1Level5();
                break;
            case 5:
                PlayerPrefs.SetInt("World1Minigame1Level", 1);
                scene.ToLevelSelection();
                break;
            default:
                break;
        }
    }

}
