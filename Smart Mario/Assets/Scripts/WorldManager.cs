using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Control for World providing scene transitions and setting player objects
/// </summary>
public class WorldManager : MonoBehaviour
{
    private SceneController scene;

    public GameObject witchPrefab;
    public GameObject knightPrefab;
    public GameObject witchClone;
    public GameObject knightClone;
    /// <summary>
    /// Get instance of SceneController once World Manager starts and sets the Player object according to player preference's selected player
    /// </summary>
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
    /// <summary>
    /// Changes scene to difficulty selection for minigame 1
    /// </summary>
    public void ToMinigame1DifficultySelection()
    {
        PlayerPrefs.SetString("Minigame Selected", "Stranded");
        scene.ToDifficultySelection();
    }
    /// <summary>
    /// Changes scene to difficulty selection for minigame 2
    /// </summary>
    public void ToMinigame2DifficultySelection()
    {
        PlayerPrefs.SetString("Minigame Selected", "Matching Cards");
        scene.ToDifficultySelection();
    }
    /// <summary>
    /// Changes scene to world selection
    /// </summary>
    public void ToWorldSelection()
    {
        scene.ToWorldSelection();
    }
}
