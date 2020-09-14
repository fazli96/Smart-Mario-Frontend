using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World1ManagerScreen : MonoBehaviour
{
    private SceneController scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.getSceneController();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToMinigame1DifficultySelection()
    {
        PlayerPrefs.SetString("Minigame Selected", "Minigame1");
        scene.ToDifficultySelection();
    }

    public void ToMinigame2DifficultySelection()
    {
        PlayerPrefs.SetString("Minigame Selected", "Minigame2");
        scene.ToDifficultySelection();
    }

    public void ToWorldSelection()
    {
        scene.ToWorldSelection();
    }
}
