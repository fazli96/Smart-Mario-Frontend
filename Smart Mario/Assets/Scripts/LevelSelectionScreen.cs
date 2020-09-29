using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScreen : MonoBehaviour
{
    public Text minigameName_LvlSel;
    public Button level2button;
    public Button level3button;
    public Button level4button;
    public Button level5button;

    private SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        string minigameSelected = PlayerPrefs.GetString("Minigame Selected");
        string difficulty = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        minigameName_LvlSel.text = "Welcome to " + minigameSelected;
        Debug.Log("World1" + minigameSelected + "HighestLevelCompleted" + difficulty);
        switch (PlayerPrefs.GetInt("World1"+minigameSelected+"HighestLevelCompleted"+difficulty, 2))
        {
            case 0:
                level2button.interactable = false;
                level3button.interactable = false;
                level4button.interactable = false;
                level5button.interactable = false;
                break;
            case 1:
                level3button.interactable = false;
                level4button.interactable = false;
                level5button.interactable = false;
                break;
            case 2:
                level4button.interactable = false;
                level5button.interactable = false;
                break;
            case 3:
                level5button.interactable = false;
                break;
            default:
                break;
        }

    }

    public void ToLevel1()
    {
        
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            PlayerPrefs.SetInt("World1Minigame1Level", 1);
            scene.ToWorld1Minigame1Level1();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            PlayerPrefs.SetInt("World1Minigame2Level", 1);
            scene.ToWorld1Minigame2Level1();
        }
    }

    public void ToLevel2()
    {
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            PlayerPrefs.SetInt("World1Minigame1Level", 2);
            scene.ToWorld1Minigame1Level2();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            PlayerPrefs.SetInt("World1Minigame2Level", 2);
            scene.ToWorld1Minigame2Level2();
        }
    }

    public void ToLevel3()
    {
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            PlayerPrefs.SetInt("World1Minigame1Level", 3);
            scene.ToWorld1Minigame1Level3();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            PlayerPrefs.SetInt("World1Minigame2Level", 3);
            scene.ToWorld1Minigame2Level3();
        }
    }

    public void ToLevel4()
    {
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            PlayerPrefs.SetInt("World1Minigame1Level", 4);
            scene.ToWorld1Minigame1Level4();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            PlayerPrefs.SetInt("World1Minigame2Level", 4);
            scene.ToWorld1Minigame2Level4();
        }
    }

    public void ToLevel5()
    {
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            PlayerPrefs.SetInt("World1Minigame1Level", 5);
            scene.ToWorld1Minigame1Level5();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            PlayerPrefs.SetInt("World1Minigame2Level", 5);
            scene.ToWorld1Minigame2Level5();
        }
    }

    public void BackToDifficultySel()
    {
        scene.ToDifficultySelection();
    }
}
