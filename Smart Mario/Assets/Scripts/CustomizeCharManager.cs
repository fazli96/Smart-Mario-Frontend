using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeCharManager : MonoBehaviour
{
    public GameObject witch;
    public GameObject knight;

    private int charInt = 1;
    private SceneController scene;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("Selected Player", "Witch").Equals("Witch"))
        {
            witch.SetActive(true);
            knight.SetActive(false);
            charInt = 1;
        }
        else if (PlayerPrefs.GetString("Selected Player", "Witch").Equals("Knight"))
        {
            witch.SetActive(false);
            knight.SetActive(true);
            charInt = 2;
        }
        scene = SceneController.getSceneController();
    }

    // Update is called once per frame
    public void NextCharacter()
    {
        switch(charInt)
        {
            case 1:
                PlayerPrefs.SetString("Selected Player", "Knight");
                knight.SetActive(true);
                witch.SetActive(false);
                charInt++;
                break;
            case 2:
                PlayerPrefs.SetString("Selected Player", "Witch");
                witch.SetActive(true);
                knight.SetActive(false);
                charInt = 1;
                break;
            default:
                break;
        }
    }
    public void PrevCharacter()
    {
        switch (charInt)
        {
            case 1:
                PlayerPrefs.SetString("Selected Player", "Knight");
                knight.SetActive(true);
                witch.SetActive(false);
                charInt++;
                break;
            case 2:
                PlayerPrefs.SetString("Selected Player", "Witch");
                witch.SetActive(true);
                knight.SetActive(false);
                charInt = 1;
                break;
            default:
                break;
        }
    }

    public void SelectPlayer()
    {
        scene.ToMainMenu();
    }
}
