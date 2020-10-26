using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This class controls the behavior of the canvas  
/// </summary>
public class MatchingMultiplayerUIManager : MonoBehaviour
{
    public Text matchText;
    Canvas canvas;
    public GameObject rulesPanel;
    private SceneController scene;
    GameObject GameManager;

    public static MatchingMultiplayerUIManager instance;

    /// <summary>
    /// This is called at the start of initialisation
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
    void Start()
    {

        canvas = GetComponent<Canvas>();
        //GameManager = GameObject.Find("GameManager");
        scene = SceneController.GetSceneController();
        //pausePanel.SetActive(false);
        //isPaused = false;
    }

   
    ///// <summary>
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
    /// <summary>
    /// This method is called when the 'Leave Game' or 'Return To Lobby' button is pressed
    /// It navigates the player back to the multiplayer lobby
    /// </summary>
    public void ToMultiplayerLobby()
    {
        NetworkManager.instance.GetComponent<NetworkManager>().CommandLeaveChallenge();
    }
}

