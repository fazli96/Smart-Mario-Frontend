using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
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
    /// Attached the username of Student to the player Object
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
        string username = PlayerPrefs.GetString("username", "1");
        switch (PlayerPrefs.GetString("customChar", "1"))
        {
            case "1":
                witchClone = Instantiate(witchPrefab,
                witchClone.transform.position,
                Quaternion.Euler(0, 0, 0)) as GameObject;
                Transform witchCloneTransform = witchClone.transform.Find("Player Name Canvas");
                Transform witchCloneTransform1 = witchCloneTransform.transform.Find("Player Name");
                Text witchPlayerName = witchCloneTransform1.GetComponent<Text>();
                witchPlayerName.text = username;
                witchClone.name = username;
                break;
            case "2":
                knightClone = Instantiate(knightPrefab,
                knightClone.transform.position,
                Quaternion.Euler(0, 0, 0)) as GameObject;
                Transform knightCloneTransform = knightClone.transform.Find("Player Name Canvas");
                Transform knightCloneTransform1 = knightCloneTransform.transform.Find("Player Name");
                Text knightPlayerName = knightCloneTransform1.GetComponent<Text>();
                knightPlayerName.text = username;
                knightClone.name = username;
                break;
            default:
                break;
        }
        
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
