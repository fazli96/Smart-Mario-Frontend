using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// This class controls the behavior of the canvas  for the Single Player Session
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
    public Animator animator;
    public AudioSource world1MatchingSound;
    public AudioSource world2MatchingSound;
    public AudioSource pauseMenuSound;

    /// <summary>
    /// This is called before the first frame to initialise the singleton
    /// </summary>
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
        if (PlayerPrefs.GetInt("World", 1) == 1)
            world1MatchingSound.Play();
        else
            world2MatchingSound.Play();

    }
    /// <summary>
    /// Update is called once per frame
    /// This method is used to check for the pause input from the user 
    /// It disables the time scale to prevent user actions except from the pause panel that is activated 
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !MatchingGameStatus.instance.gameComplete && Game2Control.instance.start)
        {
            pauseMenuSound.Play();
            if (!isPaused)
            {
                isPaused = true;
                pausePanel.SetActive(true);
                //Time.timeScale = 0;
                GameManager.GetComponent<Game2Control>().changePauseState();
                Debug.Log("Pause panel");
                if (PlayerPrefs.GetInt("World", 1) == 1)
                    world1MatchingSound.Pause();
                else
                    world2MatchingSound.Pause();

            }
            else
            {
                isPaused = false;
                pausePanel.SetActive(false);
                GameManager.GetComponent<Game2Control>().changePauseState();
                //Time.timeScale = 1;
                if (PlayerPrefs.GetInt("World", 1) == 1)
                    world1MatchingSound.Play();
                else
                    world2MatchingSound.Play();
            }
            Debug.Log("Esc pressed");
        }
    }
    /// <summary>
    /// This method is called when the 'Back To World'button is pressed
    /// </summary>
    public void ToWorld()
    {
        StartCoroutine(LoadWorldAfterTransition());
        
    }
    /// <summary>
    /// Starts a coroutine for scene transition animation
    /// </summary>
    /// <returns>WaitForSeconds</returns>
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
    /// <summary>
    /// Starts a coroutine for scene transition animation
    /// </summary>
    /// <returns>WaitForSeconds</returns>
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
    /// <summary>
    /// Starts a coroutine for scene transition animation
    /// </summary>
    /// <returns>WaitForSeconds</returns>
    IEnumerator LoadRestartAfterTransition()
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
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
