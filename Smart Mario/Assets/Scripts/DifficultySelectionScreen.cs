using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectionScreen : MonoBehaviour
{
    public Text minigameName_DiffSel;
    public Text minigameName_LvlSel;
    public GameObject DiffSelPanel;
    public GameObject LvlSelPanel;
    private SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        minigameName_DiffSel.text = "Welcome to " + PlayerPrefs.GetString("Minigame Selected");
        minigameName_LvlSel.text = "Welcome to " + PlayerPrefs.GetString("Minigame Selected");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToMinigameEasy()
    {
        PlayerPrefs.SetInt("Minigame Difficulty", 3);
        DiffSelPanel.SetActive(false);
        LvlSelPanel.SetActive(true);
    }

    public void ToMinigameMedium()
    {
        PlayerPrefs.SetInt("Minigame Difficulty", 2);
        DiffSelPanel.SetActive(false);
        LvlSelPanel.SetActive(true);
    }

    public void ToMinigameHard()
    {
        PlayerPrefs.SetInt("Minigame Difficulty", 1);
        DiffSelPanel.SetActive(false);
        LvlSelPanel.SetActive(true);
    }

    public void ToLevel1()
    {
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            scene.ToMinigame1Level1();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            scene.ToMinigame2Level1();
        }
    }

    public void ToLevel2()
    {
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            scene.ToMinigame1Level2();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            scene.ToMinigame2Level2();
        }
    }

    public void ToLevel3()
    {
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            scene.ToMinigame1Level3();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            scene.ToMinigame2Level3();
        }
    }

    public void ToLevel4()
    {
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            scene.ToMinigame1Level4();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            scene.ToMinigame2Level4();
        }
    }

    public void ToLevel5()
    {
        if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame1"))
        {
            scene.ToMinigame1Level5();
        }
        else if (PlayerPrefs.GetString("Minigame Selected").Equals("Minigame2"))
        {
            scene.ToMinigame2Level5();
        }
    }

    public void BackToDifficultySel()
    {
        DiffSelPanel.SetActive(true);
        LvlSelPanel.SetActive(false);
    }
}
