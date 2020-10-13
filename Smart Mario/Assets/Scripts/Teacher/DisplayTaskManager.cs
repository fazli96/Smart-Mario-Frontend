using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTaskManager : MonoBehaviour
{
    //Singleton
    private static DisplayTaskManager instance = null;
    
    public static DisplayTaskManager GetDisplayTaskManager()
    {
        if (instance == null)
        {
            instance = new DisplayTaskManager();
        }
        return instance;
    }
}
