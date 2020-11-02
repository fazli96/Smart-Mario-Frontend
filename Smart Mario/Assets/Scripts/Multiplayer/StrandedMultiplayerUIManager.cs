using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class manages the UI elements present when the player is in Minigame Stranded.
/// </summary>
public class StrandedMultiplayerUIManager : MonoBehaviour
{
    private SceneController scene;
    public AudioSource world1StrandedSound;
    public AudioSource world2StrandedSound;
    public Animator animator;

    /// <summary>
    /// This is called before the first frame update to get the instance of scene Controller
    /// </summary>
    void Start()
    {
        if (PlayerPrefs.GetInt("World", 1) == 1)
            world1StrandedSound.Play();
        else
            world2StrandedSound.Play();
        scene = SceneController.GetSceneController();
    }

    /// <summary>
    /// This is to stop playing the sound when the game is completed.
    /// </summary>
    void Update()
    {
        if (StrandedMultiplayerGameManager.levelComplete)
        {
            if (PlayerPrefs.GetInt("World", 1) == 1)
                world1StrandedSound.Stop();
            else
                world2StrandedSound.Stop();
        }
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
