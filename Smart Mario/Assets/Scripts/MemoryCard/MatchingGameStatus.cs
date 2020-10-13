using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class MatchingGameStatus : MonoBehaviour
{
    //public Text timeText;
    public GameObject timeText;

    private static float time;
    private static int currentScore;
    private static int qnsAttempted;
    private static int qnsAnsweredCorrectly;
    private bool paused;

    GameObject GameManager;

    private static readonly string url = "https://smart-mario-backend-1.herokuapp.com/api/results";

    public static MatchingGameStatus instance;

    // Start is called before the first frame update
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
    void Update()
    {
        if (!paused)
        {
            time += Time.deltaTime;
        }
        timeText.GetComponent<UnityEngine.UI.Text>().text = "Time : "+ time.ToString("F2");
    }
    public void Pause(bool p)
    {
        paused = p;
    }
    
    public void Initialize()
    {
        qnsAttempted = 0;
        qnsAnsweredCorrectly = 0;
        GameManager = GameObject.Find("GameManager");
        Debug.Log("found game manager");
    }
    public void ScoreIncrease()
    {
        qnsAnsweredCorrectly += 1;
        qnsAttempted += 1;
        Debug.Log("state : " + qnsAttempted + " " + qnsAnsweredCorrectly);
    }
    public void QnsAttemptIncrease()
    {
        qnsAttempted += 1;
        Debug.Log("state : " + qnsAttempted + " " + qnsAnsweredCorrectly);
    }
    public void WinLevel()
    {
        paused = true;
        GameManager.GetComponent<Game2Control>().Win(time, qnsAnsweredCorrectly, qnsAttempted);
    }
    //code to put results into DB...
}
