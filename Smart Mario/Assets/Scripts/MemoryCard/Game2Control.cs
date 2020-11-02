using System;
using System.Collections;
using System.Collections.Generic;
using System.Media;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class checks the logic of minigame 2 as well as keeping track of the states in the game
/// </summary>
public class Game2Control : MonoBehaviour
{
    GameObject questionManager;
    GameObject cardManager;
    GameObject canvas;
    public bool paused;
    public bool disable;
    public bool start;
    int [] visibleFaces = new int[2];
    public GameObject finishText;
    public GameObject overlay;
    public GameObject time;
    public GameObject qns;
    public GameObject timeScore;
    public GameObject accScore;
    public GameObject backButton;
    public GameObject rulesText;
    public GameObject total;
    public GameObject totalScore;
    public GameObject finishPanel;

    public static Game2Control instance;

    public AudioSource cardMatchingSound;
    public AudioSource cardOpenSound;
    public AudioSource cardCloseSound;
    public AudioSource winLevelSound;

    public int scoreValue;
    /// <summary>
    /// Called at the start of script initialisation
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
        Time.timeScale = 1;
        questionManager = GameObject.Find("QuestionManager");
        cardManager = GameObject.Find("CardsManager");
        canvas = GameObject.Find("Canvas");
    }
    /// <summary>
    /// This is called before the first frame update
    /// </summary>
    void Start()
    {
        finishPanel.SetActive(false);

        scoreValue = cardManager.GetComponent<CardsManager>().pairs;
        paused = true;
        disable = false;
        start = false;
        visibleFaces[0] = -1;
        visibleFaces[1] = -2;
        Debug.Log("Entering new game");

        MatchingGameStatus.instance.Initialize();
        if (PlayerPrefs.GetInt("MinigameLevel") == 5)
        {
            rulesText.GetComponent<UnityEngine.UI.Text>().text = "Objective:\nUncover boundless treasure for your player by opening up the treasure chests. Clear the field by pairing two cards with matching descriptions, and be the fastest to find all the pairs\n" +
                "\nRules:\nClick on one chest to open it up and reveal the short description.Open up more chests to match the two descriptions together.If the chests don't match, they will close. Match all the pairs of descriptions to win. Try to take the shortest time possible to match all the cards.\n" +
                "\nControls: \nLeft click on chest to reveal the description\nPress the 'Esc' key to pause the game";
        }
    }
    /// <summary>
    /// This is called when the player pauses the game
    /// It pauses the logic of the game and propagates the pause state to other singleton classes
    /// </summary>
    public void changePauseState()
    {
        if (!start)
            start = true;
        paused = !paused;
        MatchingGameStatus.instance.Pause(paused);
        Debug.Log("Paused " + paused);
    }
    /// <summary>
    /// Checks the state of the game whether two cards have been flipped up
    /// </summary>
    /// <returns>Boolean on whether there are two flipped cards</returns>
    public bool TwoCards()
    {
    
        return (visibleFaces[0] >= 0 && visibleFaces[1] >= 0);

    }
    /// <summary>
    /// Adds a face to a list of already visible faces
    /// Called when a card has been flipped up
    /// Subsequently shows the questions corresponding to the card
    /// </summary>
    /// <param name="index"></param>
    /// <param name="qOrA"></param>
    public void AddVisibleFace(int index, int qOrA)
    {
        if (visibleFaces[0]== -1)
        {
            visibleFaces[0] = index;
            questionManager.GetComponent<questionManager>().showQuestion(1, index, qOrA);
        }
        else if (visibleFaces[1] == -2)
        {
            visibleFaces[1] = index;
            questionManager.GetComponent<questionManager>().showQuestion(2, index, qOrA);
        }
    }
    /// <summary>
    /// Removes a face from the list of visible faces
    /// Also hides the corresponding question of the hidden card
    /// </summary>
    /// <param name="index"></param>
    public void RemoveVisibleFace(int index)
    {
        if (visibleFaces[0] == index)
        {
            visibleFaces[0] = -1;
            questionManager.GetComponent<questionManager>().hideQuestion(1, false);
        }
        else if (visibleFaces[1] == index)
        {
            visibleFaces[1] = -2;
            questionManager.GetComponent<questionManager>().hideQuestion(2, false);
        }
    }
    /// <summary>
    /// Called when there is match between two cards
    /// </summary>
    /// <param name="index"></param>
    public void ShowMatch(int index)
    {
        canvas.GetComponent<CanvasControl>().ShowMatch();
        PlayMatchSound();
        StartCoroutine(RightCards(index));

    }
    /// <summary>
    /// Interface to set up a coroutine to delay hiding the questions after a match
    /// </summary>
    /// <param name="index"></param>
    /// <returns>Yield of waiting for seconds</returns>
    IEnumerator RightCards(int index)
    {
        Debug.Log(Time.time);
        disable = true;
        yield return new WaitForSeconds((float)1.5);
        Debug.Log(Time.time);
        cardManager.GetComponent<CardsManager>().faceindexes.Add(index);
        cardManager.GetComponent<CardsManager>().Open(index);
        questionManager.GetComponent<questionManager>().hideQuestion(1, true);
        questionManager.GetComponent<questionManager>().hideQuestion(2, true);
        disable = false;
        
        if (scoreValue == 0)
        {
            PlayWinSound();
            MatchingGameStatus.instance.WinLevel();
        }
    }
   /// <summary>
   /// Checks whether two visible faces are matching or not
   /// </summary>
   /// <returns>Boolean on match of two cards</returns>
    public bool CheckMatch()
    {
        if (visibleFaces[0]== visibleFaces[1])
        {
            visibleFaces[0] = -1;
            visibleFaces[1] = -2;
            scoreValue -= 1;
            MatchingGameStatus.instance.ScoreIncrease();
            Debug.Log("pairs " + scoreValue);
            return true;
        }
        else
        {
            StartCoroutine(WrongCards());
            
            return false;
        }
    }
    /// <summary>
    /// Interface to set up coroutine to delay hiding cards in the case of wrong attemp
    /// </summary>
    /// <returns>Yield of wait for seconds</returns>
    IEnumerator WrongCards()
    {
        Debug.Log(Time.time);
        yield return new WaitForSeconds(2);
        Debug.Log(Time.time);
        if (TwoCards())
        {
            MatchingGameStatus.instance.QnsAttemptIncrease();
            foreach (int index in visibleFaces)
            {
                RemoveVisibleFace(index);
                cardManager.GetComponent<CardsManager>().Close(index);
            }
        }
    }

    /// <summary>
    /// This method is called when the game has ended
    /// </summary>
    /// <param name="t"></param>
    /// <param name="correct"></param>
    /// <param name="attempt"></param>
    /// <param name="accSc"></param>
    /// <param name="timeSc"></param>
    public void Win(float t, int correct, int attempt, int accSc, int timeSc)
    {
        float accuracy = ((float)correct / (float)attempt) *100;
        Debug.Log(t + " " + correct + " " + attempt + " " + accuracy);
        qns.GetComponent<UnityEngine.UI.Text>().text = "Accuracy: " + accuracy.ToString("F2") +"%";
        time.GetComponent<UnityEngine.UI.Text>().text = "Time taken: " + t.ToString("F2")+ " sec";
        timeScore.GetComponent<UnityEngine.UI.Text>().text =  timeSc.ToString()+ " Points";
        accScore.GetComponent<UnityEngine.UI.Text>().text = accSc.ToString()+ " Points";
        totalScore.GetComponent<UnityEngine.UI.Text>().text = (timeSc + accSc).ToString() + " Points";

        finishPanel.SetActive(true);

    }

    public void PlayMatchSound()
    {
        cardMatchingSound.Play();
    }
    public void PlayOpenSound()
    {
        cardOpenSound.Play();
    }
    public void PlayCloseSound()
    {
        cardCloseSound.Play();
    }
    public void PlayWinSound()
    {
        winLevelSound.Play();
    }


}
