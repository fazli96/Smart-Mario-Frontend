using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is for managing the message log of player interactions during the multiplayer challenge
/// </summary>
public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager instance;

    public GameObject messageLogPanel;
    public GameObject textObject;

    public int maxMessages = 15;
    public List<Message> messageList = new List<Message>();
    
    /// <summary>
    /// This method is called before the first frame update to initialize the message log to empty
    /// </summary>
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
        messageList.Clear();
    }

    /// <summary>
    /// This method is called to append new messages of player interactions to the message log
    /// Message Log can only store up to a certain number of messages where the least recent messages are disposed of when limit exceeded
    /// </summary>
    /// <param name="text"></param>
    public void SendToMessageLog(string text)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);

        }
            
        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textObject, messageLogPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }

}

/// <summary>
/// This class is for storing the message received from the server to be displayed on the message log
/// </summary>
[Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
