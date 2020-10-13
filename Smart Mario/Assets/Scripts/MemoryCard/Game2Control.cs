using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// This class checks the logic of minigame 2 as well as keeping track of the states in the game
/// </summary>
public class Game2Control : MonoBehaviour
{
    GameObject questionManager;
    GameObject cardManager;
    GameObject canvas;
    public bool paused;
    int[] visibleFaces = { -1, -2 };
    public GameObject finishText;
    public GameObject overlay;
    public GameObject time;
    public GameObject qns;
    
    public int scoreValue;
    /// <summary>
    /// Called at the start of script initialisation
    /// </summary>
    void Awake()
    {
        questionManager = GameObject.Find("QuestionManager");
        cardManager = GameObject.Find("CardsManager");
        canvas = GameObject.Find("Canvas");
    }
    void Start()
    {
        finishText.SetActive(false);
        overlay.SetActive(false);
        qns.SetActive(false);
        time.SetActive(false);
        scoreValue = cardManager.GetComponent<CardsManager>().pairs;
        paused = false;
        MatchingGameStatus.instance.Initialize();
    }
    public void changePauseState(bool p)
    {
        paused = p;
        MatchingGameStatus.instance.Pause(p);
        Debug.Log("Paused " + p);

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
        StartCoroutine(RightCards(index));
    }
    IEnumerator RightCards(int index)
    {
        Debug.Log(Time.time);
        yield return new WaitForSeconds(2);
        Debug.Log(Time.time);
        cardManager.GetComponent<CardsManager>().faceindexes.Add(index);
        cardManager.GetComponent<CardsManager>().Open(index);
        questionManager.GetComponent<questionManager>().hideQuestion(1, true);
        questionManager.GetComponent<questionManager>().hideQuestion(2, true);
        if (scoreValue == 0)
        {
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

    public void Win(float t, int correct, int attempt)
    {
        float accuracy = ((float)correct / (float)attempt) *100;
        Debug.Log(t + " " + correct + " " + attempt + " " + accuracy);
        qns.GetComponent<UnityEngine.UI.Text>().text = "Accuracy: " + accuracy.ToString("F2") +"%";
        time.GetComponent<UnityEngine.UI.Text>().text = "Time taken: " + t.ToString("F2")+ " sec";
        finishText.SetActive(true);
        overlay.SetActive(true);
        time.SetActive(true);
        qns.SetActive(true);

    }
    

}
