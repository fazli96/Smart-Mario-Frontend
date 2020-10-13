using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class controls the behavior of the canvas  
/// </summary>
public class CanvasControl : MonoBehaviour
{
    public Text matchText;
    Canvas canvas;
    public GameObject pausePanel;
    public GameObject rulesPanel;
    public bool isPaused;
    private SceneController scene;
    GameObject GameManager;
    /// <summary>
    /// This is called at the start of initialisation
    /// </summary>
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        GameManager = GameObject.Find("GameManager");
        scene = SceneController.GetSceneController();
        pausePanel.SetActive(false);
        isPaused = false;
    }
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                isPaused = true;
                pausePanel.SetActive(true);
                Time.timeScale = 0;
                GameManager.GetComponent<Game2Control>().changePauseState(isPaused);
                Debug.Log("Pause panel");
            }
            else
            {
                isPaused = false;
                pausePanel.SetActive(false);
                GameManager.GetComponent<Game2Control>().changePauseState(isPaused);
                Time.timeScale = 1;
            }
            Debug.Log("Esc pressed");
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
        switch (PlayerPrefs.GetInt("World1Minigame2Level", 1))
        {
            case 1:
                scene.ToWorld1Minigame2Level1();
                break;
            case 2:
                scene.ToWorld1Minigame2Level2();
                break;
            case 3:
                scene.ToWorld1Minigame2Level3();
                break;
            case 4:
                scene.ToWorld1Minigame2Level4();
                break;
            case 5:
                scene.ToWorld1Minigame2Level5();
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
        switch (PlayerPrefs.GetInt("World1Minigame2Level", 1))
        {

            case 1:
                PlayerPrefs.SetInt("World1Minigame2Level", 2);
                scene.ToWorld1Minigame1Level2();
                break;
            case 2:
                PlayerPrefs.SetInt("World1Minigame2Level", 3);
                scene.ToWorld1Minigame1Level3();
                break;
            case 3:
                PlayerPrefs.SetInt("World1Minigame2Level", 4);
                scene.ToWorld1Minigame1Level4();
                break;
            case 4:
                PlayerPrefs.SetInt("World1Minigame2Level", 5);
                scene.ToWorld1Minigame1Level5();
                break;
            case 5:
                PlayerPrefs.SetInt("World1Minigame2Level", 1);
                scene.ToLevelSelection();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// This class instantiates text that shows a match between two cards
    /// </summary>
    public void ShowMatch()
    {
        Text one = Instantiate(matchText, new Vector2(          //instantiating prefab
                350, 80),
                Quaternion.identity);
        one.transform.SetParent(canvas.transform, false);
        Text two = Instantiate(matchText, new Vector2(          //instantiating prefab
                350, -100),
                Quaternion.identity);
        two.transform.SetParent(canvas.transform, false);
        
    }
}
