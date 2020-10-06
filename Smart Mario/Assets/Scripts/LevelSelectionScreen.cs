using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScreen : MonoBehaviour
{
    public Text minigameNameText;
    public Button level2button;
    public Button level3button;
    public Button level4button;
    public Button level5button;

    private int worldSelected;
    private string minigameSelected;
    private string difficulty;

    private SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        worldSelected = PlayerPrefs.GetInt("World", 1);
        minigameSelected = PlayerPrefs.GetString("Minigame Selected", "Stranded");
        difficulty = PlayerPrefs.GetString("Minigame Difficulty", "Easy");
        minigameNameText.text = "Welcome to " + minigameSelected;

        /*Debug.Log("World"+ worldSelected + minigameSelected + "HighestLevelCompleted" + difficulty);
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
        }*/

    }

    public void ToLevel1()
    {
        PlayerPrefs.SetInt("MinigameLevel", 1);
        if (worldSelected == 1)
        {
            if (minigameSelected.Equals("Stranded"))
                scene.ToWorld1Minigame1Level1();
            else
                scene.ToWorld1Minigame2Level1();
        }
        else
        {
            if (minigameSelected.Equals("Stranded"))
                scene.ToWorld2Minigame1Level1();
            //else
            //scene.ToWorld2Minigame2Level1();
        }
    }

    public void ToLevel2()
    {
        PlayerPrefs.SetInt("MinigameLevel", 2);
        if (worldSelected == 1)
        {
            if (minigameSelected.Equals("Stranded"))
                scene.ToWorld1Minigame1Level2();
            else
                scene.ToWorld1Minigame2Level2();
        }
        else
        {
            if (minigameSelected.Equals("Stranded"))
                scene.ToWorld2Minigame1Level2();
            //else
            //scene.ToWorld2Minigame2Level2();
        }
    }

    public void ToLevel3()
    {
        PlayerPrefs.SetInt("MinigameLevel", 3);
        if (worldSelected == 1)
        {
            if (minigameSelected.Equals("Stranded"))
                scene.ToWorld1Minigame1Level3();  
            else
                scene.ToWorld1Minigame2Level3();
        }
        else
        {
            if (minigameSelected.Equals("Stranded"))
                scene.ToWorld2Minigame1Level3();
            //else
            //scene.ToWorld2Minigame2Level3();
        }
    }

    public void ToLevel4()
    {
        PlayerPrefs.SetInt("MinigameLevel", 4);
        if (worldSelected == 1)
        {
            if (minigameSelected.Equals("Stranded"))
                scene.ToWorld1Minigame1Level4();
            else
                scene.ToWorld1Minigame2Level4();
        }
        else
        {
            if (minigameSelected.Equals("Stranded"))
                scene.ToWorld2Minigame1Level4();
            //else
            //scene.ToWorld2Minigame2Level4();
        }
    }

    public void ToLevel5()
    {
        PlayerPrefs.SetInt("MinigameLevel", 5);
        if (worldSelected == 1)
        {
            if (minigameSelected.Equals("Stranded"))
                scene.ToWorld1Minigame1Level5();
            else
                scene.ToWorld1Minigame2Level5();
        }
        else
        {
            if (minigameSelected.Equals("Stranded"))
                scene.ToWorld2Minigame1Level5();
            //else
            //scene.ToWorld2Minigame2Level5();
        }
    }

    public void BackToDifficultySel()
    {
        scene.ToDifficultySelection();
    }
}
