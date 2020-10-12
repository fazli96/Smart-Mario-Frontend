using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{
    public static ChallengeManager instance;

    public GameObject messageLogPanel;
    public GameObject textObject;

    public int maxMessages = 15;
    public List<Message> messageList = new List<Message>();
    
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
        messageList.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

[Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
