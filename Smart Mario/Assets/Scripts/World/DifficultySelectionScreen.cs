using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class contains all methods that manages the UI elements in the difficulty sleection page
/// </summary>
public class DifficultySelectionScreen : MonoBehaviour
{
    public Text minigameNameText;
    private SceneController scene;
    /// <summary>
    ///  This method is called before the first frame update
    /// </summary>
    void Start()
    {
        scene = SceneController.GetSceneController();
        minigameNameText.text = "Welcome to " + PlayerPrefs.GetString("Minigame Selected");

    }

    /// <summary>
    /// This method is called when 'Easy' button is pressed
    /// It navigates the player to the level selection page
    /// </summary>
    public void ToMinigameEasy()
    {
        PlayerPrefs.SetString("Minigame Difficulty", "Easy");
        scene.ToLevelSelection();
    }

    /// <summary>
    /// This method is called when 'Medium' button is pressed
    /// It navigates the player to the level selection page
    /// </summary>
    public void ToMinigameMedium()
    {
        PlayerPrefs.SetString("Minigame Difficulty", "Medium");
        scene.ToLevelSelection();
    }

    /// <summary>
    /// This method is called when 'Hard' button is pressed
    /// It navigates the player to the level selection page
    /// </summary>
    public void ToMinigameHard()
    {
        PlayerPrefs.SetString("Minigame Difficulty", "Hard");
        scene.ToLevelSelection();
    }

    /// <summary>
    /// This method is called when'Easy' button is pressed
    /// It navigates the player to the World map page
    /// </summary>
    public void BackToWorld()
    {
        if (PlayerPrefs.GetInt("World", 1) == 1)
            scene.PlayWorld1();
        else
            scene.PlayWorld2();
    }
}
