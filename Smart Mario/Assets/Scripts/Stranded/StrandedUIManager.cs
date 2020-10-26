using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class manages the UI elements present when the player is in Minigame Stranded.
/// </summary>
public class StrandedUIManager : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject rulesPanel;
    private bool isPaused;
    private SceneController scene;

    /// <summary>
    /// This is called before the first frame update to get the instance of scene Controller and to hide the pause menu on start
    /// </summary>
    void Start()
    {
        scene = SceneController.GetSceneController();
        pausePanel.SetActive(false);
        isPaused = false;
    }

    /// <summary>
    /// For every frame, if player is not encountering a question and the Escape key is pressed, show the pause menu
    /// </summary>
    void Update()
    {  
        if (Input.GetKeyDown(KeyCode.Escape) && !StrandedGameManager.qnEncountered
            && !StrandedGameManager.levelComplete)
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
        scene.ToWorld1Stranded();
    }

    /// <summary>
    /// This method is called when the 'Next Level'button is pressed
    /// </summary>
    public void NextLevel()
    {
        int world = PlayerPrefs.GetInt("World", 1);
        int level = PlayerPrefs.GetInt("MinigameLevel", 1);

        switch (level)
        {
            case 1:
                PlayerPrefs.SetInt("MinigameLevel", 2);
                break;
            case 2:
                PlayerPrefs.SetInt("MinigameLevel", 3);
                break;
            case 3:
                PlayerPrefs.SetInt("MinigameLevel", 4);
                break;
            case 4:
                PlayerPrefs.SetInt("MinigameLevel", 5);
                break;
            case 5:
                PlayerPrefs.SetInt("MinigameLevel", 1);
                break;
            default:
                break;
        }
        // if level 5 is completed, next level will direct player to level selection screen
        if (level == 5)
            scene.ToLevelSelection();
        // go to next level based on world selected
        else if (world == 1)
            scene.ToWorld1Stranded();
        else
            scene.ToWorld2Stranded();
    }

}
