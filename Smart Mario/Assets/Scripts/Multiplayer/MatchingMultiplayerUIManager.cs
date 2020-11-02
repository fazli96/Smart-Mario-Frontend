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
    private SceneController scene;
    GameObject GameManager;

    public static MatchingMultiplayerUIManager instance;
    public Animator animator;
    public AudioSource world1MatchingSound;
    public AudioSource world2MatchingSound;

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
    /// <summary>
    /// This is called at the start of initialisation
    /// </summary>
    /// 
    void Start()
    {
        canvas = GetComponent<Canvas>();
        scene = SceneController.GetSceneController();
        if (PlayerPrefs.GetString("Minigame Selected", "World 1 Matching Cards") == "World 1 Matching Cards")
            world1MatchingSound.Play();
        else
            world2MatchingSound.Play();
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
        StartCoroutine(LoadLobbyAfterTransition());
    }
    IEnumerator LoadLobbyAfterTransition()
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        NetworkManager.instance.GetComponent<NetworkManager>().CommandLeaveChallenge();
    }
}

