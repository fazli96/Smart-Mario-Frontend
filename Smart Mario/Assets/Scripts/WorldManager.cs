using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private SceneController scene;

    public GameObject witchPrefab;
    public GameObject knightPrefab;
    public GameObject witchClone;
    public GameObject knightClone;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        switch (PlayerPrefs.GetString("Selected Player", "Witch"))
        {
            case "Witch":
                witchClone = Instantiate(witchPrefab,
                witchClone.transform.position,
                Quaternion.Euler(0, 0, 0)) as GameObject;
                break;
            case "Knight":
                knightClone = Instantiate(knightPrefab,
                knightClone.transform.position,
                Quaternion.Euler(0, 0, 0)) as GameObject;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToMinigame1DifficultySelection()
    {
        PlayerPrefs.SetString("Minigame Selected", "Stranded");
        scene.ToDifficultySelection();
    }

    public void ToMinigame2DifficultySelection()
    {
        PlayerPrefs.SetString("Minigame Selected", "Matching Cards");
        scene.ToDifficultySelection();
    }

    public void ToWorldSelection()
    {
        scene.ToWorldSelection();
    }
}
