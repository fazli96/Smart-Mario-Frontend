using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class manages the UI elements present when the player is in Minigame Stranded.
/// </summary>
public class Minigame1_UIManager : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject rulesPanel;
    private bool isPaused;
    private SceneController scene;

    /// <summary>
    /// This is called before the first frame update
    /// </summary>
    void Start()
    {
        scene = SceneController.GetSceneController();
        pausePanel.SetActive(false);
        isPaused = false;
        if (GameObject.Find("NetworkManager") != null)
            rulesPanel.SetActive(false);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (GameObject.Find("NetworkManager") == null)
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
    }

    /// <summary>
    /// This method is called when the 'Back To World'button is pressed
    /// </summary>
    public void ToWorld()
    {
        if (PlayerPrefs.GetInt("World", 1) == 1)
            scene.PlayWorld1();
        else
            scene.PlayWorld2();
    }

    /// <summary>
    /// This method is called when the 'Level Selection'button is pressed
    /// </summary>
    public void ToLevelSelection()
    {
        scene.ToLevelSelection();
    }

    /// <summary>
    /// This method is called when the 'Restart Level'button is pressed
    /// </summary>
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

    /// <summary>
    /// This method is called when the 'Next Level'button is pressed
    /// </summary>
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
