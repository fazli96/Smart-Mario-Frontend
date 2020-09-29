using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectionScreen : MonoBehaviour
{
    public Text minigameName_DiffSel;
    private SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        minigameName_DiffSel.text = "Welcome to " + PlayerPrefs.GetString("Minigame Selected");

    }

    public void ToMinigameEasy()
    {
        PlayerPrefs.SetString("Minigame Difficulty", "Easy");
        scene.ToLevelSelection();
    }

    public void ToMinigameMedium()
    {
        PlayerPrefs.SetString("Minigame Difficulty", "Medium");
        PlayerPrefs.SetInt("World1Minigame1HighestLevelCompletedMedium", 0);
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
