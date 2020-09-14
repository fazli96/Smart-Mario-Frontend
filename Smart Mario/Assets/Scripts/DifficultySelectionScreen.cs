using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectionScreen : MonoBehaviour
{
    public Text minigameName;
    private SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.getSceneController();
        minigameName.text = "Welcome to " + PlayerPrefs.GetString("Minigame Selected");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToMinigameEasy()
    {
        PlayerPrefs.SetInt("Minigame Difficulty", 3);
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            Debug.Log("in Minigame2");
            scene.ToMinigame1();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            Debug.Log("in Minigame2");
            scene.ToMinigame2();
        }
    }

    public void ToMinigameMedium()
    {
        PlayerPrefs.SetInt("Minigame Difficulty", 2);
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            scene.ToMinigame1();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            scene.ToMinigame2();
        }
    }

    public void ToMinigameHard()
    {
        PlayerPrefs.SetInt("Minigame Difficulty", 1);
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            scene.ToMinigame1();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            scene.ToMinigame2();
        }
    }
}
