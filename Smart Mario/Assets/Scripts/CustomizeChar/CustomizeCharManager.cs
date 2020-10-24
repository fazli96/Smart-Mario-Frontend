using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Boundary that connects to Unity Customize Character Scene UI objects and triggers function calls on events
/// </summary>
public class CustomizeCharManager : MonoBehaviour
{
    public GameObject witch;
    public GameObject knight;

    private int charInt = 1;
    private SceneController scene;
    /// <summary>
    /// Set charcter shown on screen based on currently selected character
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("customChar", "1").Equals("1"))
        {
            witch.SetActive(true);
            knight.SetActive(false);
            charInt = 1;
        }
        else if (PlayerPrefs.GetString("customChar", "1").Equals("2"))
        {
            witch.SetActive(false);
            knight.SetActive(true);
            charInt = 2;
        }
        scene = SceneController.GetSceneController();
    }
    /// <summary>
    /// Changes to next character available in selection
    /// </summary>
    // Update is called once per frame
    public void NextCharacter()
    {
        switch(charInt)
        {
            case 1:
                //PlayerPrefs.SetString("Selected Player", "Knight");
                knight.SetActive(true);
                witch.SetActive(false);
                charInt = 2;
                break;
            case 2:
                //PlayerPrefs.SetString("Selected Player", "Witch");
                witch.SetActive(true);
                knight.SetActive(false);
                charInt = 1;
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Changes to previous character available in selection
    /// </summary>
    public void PrevCharacter()
    {
        switch (charInt)
        {
            case 1:
                //PlayerPrefs.SetString("Selected Player", "Knight");
                knight.SetActive(true);
                witch.SetActive(false);
                charInt = 2;
                break;
            case 2:
                //PlayerPrefs.SetString("Selected Player", "Witch");
                witch.SetActive(true);
                knight.SetActive(false);
                charInt = 1;
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Sends chosen character data to database and checks for success response
    /// </summary>
    public void SelectPlayer()
    {
        APICall api = APICall.getAPICall();
        StartCoroutine(api.CustomCharacterPutRequest(PlayerPrefs.GetString("id"), charInt.ToString()));
    }
}
