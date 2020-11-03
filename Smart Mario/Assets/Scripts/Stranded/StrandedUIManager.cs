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
    private bool isPaused;
    private SceneController scene;
    public AudioSource world1StrandedSound;
    public AudioSource world2StrandedSound;
    public AudioSource pauseMenuSound;
    public Animator animator;

    /// <summary>
    /// This is called before the first frame update to get the instance of scene Controller and to hide the pause menu on start
    /// </summary>
    void Start()
    {
        scene = SceneController.GetSceneController();
        pausePanel.SetActive(false);
        isPaused = false;
        if (PlayerPrefs.GetInt("World", 1) == 1)
            world1StrandedSound.Play();
        else
            world2StrandedSound.Play();

    }

    /// <summary>
    /// For every frame, if player is not encountering a question and the Escape key is pressed, show the pause menu
    /// </summary>
    void Update()
    {  
        if (Input.GetKeyDown(KeyCode.Escape) && !StrandedGameManager.qnEncountered
            && !StrandedGameManager.levelComplete)
        {
            pauseMenuSound.Play();
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
        if (StrandedGameManager.levelComplete)
        {
            if (PlayerPrefs.GetInt("World", 1) == 1)
                world1StrandedSound.Stop();
            else
                world2StrandedSound.Stop();
        }
    }

    /// <summary>
    /// This method is called when the 'Back To World'button is pressed
    /// </summary>
    public void ToWorld()
    {
        StartCoroutine(LoadWorldAfterTransition());

    }
    IEnumerator LoadWorldAfterTransition()
    {
        if (PlayerPrefs.GetInt("World", 1) == 1)
        {
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1f);
            scene.PlayWorld1();
        }
        else
        {
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1f);
            scene.PlayWorld2();
        }
    }


    /// <summary>
    /// This method is called when the 'Level Selection'button is pressed
    /// </summary>
    public void ToLevelSelection()
    {
        StartCoroutine(LoadLevelSelectAfterTransition());
    }
    IEnumerator LoadLevelSelectAfterTransition()
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        scene.ToLevelSelection();
    }


    /// <summary>
    /// This method is called when the 'Restart Level'button is pressed
    /// </summary>
    public void RestartLevel()
    {
        StartCoroutine(LoadRestartAfterTransition());
    }
    IEnumerator LoadRestartAfterTransition()
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            StartCoroutine(LoadNextLevelAfterTransition(level, world));
    }
    IEnumerator LoadNextLevelAfterTransition(int level, int world)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
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
