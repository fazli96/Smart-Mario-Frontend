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
    public static WorldManager instance;

    public List<GameObject> characterPrefabs = new List<GameObject>();
    public List<GameObject> characterClones = new List<GameObject>();

    public AudioSource world1Sound;
    public AudioSource world2Sound;
    /// <summary>
    /// Get instance of SceneController once World Manager starts and sets the Player object according to player preference's selected player
    /// Attached the username of Student to the player Object
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.GetInt("World", 1) == 1)
            world1Sound.Play();
        else
            world2Sound.Play();
        scene = SceneController.GetSceneController();
        string username = PlayerPrefs.GetString("username", "1");
        int customChar = int.Parse(PlayerPrefs.GetString("customChar", "0"));
        
        characterClones[customChar] = Instantiate(characterPrefabs[customChar],
        characterClones[customChar].transform.position,
        Quaternion.Euler(0, 0, 0)) as GameObject;
        Transform CloneTransform = characterClones[customChar].transform.Find("Player Name Canvas");
        Transform CloneTransform1 = CloneTransform.transform.Find("Player Name");
        Text playerName = CloneTransform1.GetComponent<Text>();
        playerName.text = username;
        characterClones[customChar].name = username;

        Time.timeScale = 1;

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
        //PlayerPrefs.SetString("Minigame Selected", "Matching Cards");
        if (PlayerPrefs.GetInt("World", 1) == 1)
        {
            PlayerPrefs.SetString("Minigame Selected", "World 1 Matching Cards");
        }
        else
            PlayerPrefs.SetString("Minigame Selected", "World 2 Matching Cards");
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
