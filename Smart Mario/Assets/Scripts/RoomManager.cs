using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{

    public static RoomManager instance;
    public Text roomNameText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        roomNameText.text = "Room: " + PlayerPrefs.GetString("roomName", "nil");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartChallenge()
    {

    }

    public void LeaveRoom()
    {
        NetworkManager.instance.GetComponent<NetworkManager>().CommandLeaveRoom();
    }
}
