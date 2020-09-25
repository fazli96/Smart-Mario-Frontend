using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class APICall
{
    //Singleton
    private static APICall instance = null;


    public static APICall getAPICall()
    {
        if (instance == null)
        {
            instance = new APICall();
        }
        return instance;
    }

    public string saveToJSONString(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public IEnumerator PostRequest(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.chunkedTransfer = false;
        UnityEngine.Debug.Log(bodyJsonString);
        yield return request.SendWebRequest();

        UnityEngine.Debug.Log("Response: " + request.downloadHandler.text);
    }
}


 
