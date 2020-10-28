﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public static CanvasControl instance;
    /// <summary>
    /// This is called before the first frame to initialise the singleton
    /// </summary>
    /// 
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// This is called at the start of initialisation
    /// </summary>
    /// 
    void Start()
    {
        
        canvas = GetComponent<Canvas>();
        GameManager = GameObject.Find("GameManager");
        scene = SceneController.GetSceneController();
        pausePanel.SetActive(false);
        isPaused = false;
    }
    /// <summary>
    /// Update is called once per frame
    /// This method is used to check for the pause input from the user 
    /// It disables the time scale to prevent user actions except from the pause panel that is activated 
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
                GameManager.GetComponent<Game2Control>().changePauseState();
                Debug.Log("Pause panel");
            }
            else
            {
                isPaused = false;
                pausePanel.SetActive(false);
                GameManager.GetComponent<Game2Control>().changePauseState();
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
