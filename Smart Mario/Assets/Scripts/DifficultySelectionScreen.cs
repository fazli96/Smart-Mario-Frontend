using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectionScreen : MonoBehaviour
{
    public Text minigameNameText;
    private SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        minigameNameText.text = "Welcome to " + PlayerPrefs.GetString("Minigame Selected");

    }

    public void ToMinigameEasy()
    {
        PlayerPrefs.SetString("Minigame Difficulty", "Easy");
        scene.ToLevelSelection();
    }

    public void ToMinigameMedium()
    {
        PlayerPrefs.SetString("Minigame Difficulty", "Medium");
        scene.ToLevelSelection();
    }

    public void ToMinigameHard()
    {
        PlayerPrefs.SetString("Minigame Difficulty", "Hard");
        scene.ToLevelSelection();
    }

    public void BackToWorld()
    {
        scene.PlayWorld1();
    }
}
