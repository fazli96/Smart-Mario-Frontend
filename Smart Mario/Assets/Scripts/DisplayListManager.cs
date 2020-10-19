using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DisplayListManager
{
    //Singleton
    private static DisplayListManager instance = null;
    
    private List<DisplayResults> displayResultsList;
    
    public static DisplayListManager GetDisplayListManager()
    {
        if (instance == null)
        {
            instance = new DisplayListManager();
        }
        return instance;
    }


    public void RetrieveData(string result)
    {
        displayResultsList = new List<DisplayResults>();
        JArray data = (JArray)JsonConvert.DeserializeObject(result);
        foreach (JObject one_resultJobj in data)
        {
            DisplayResults one_result = one_resultJobj.ToObject<DisplayResults>();
            one_result.studentName = one_resultJobj["student"]["name"].ToString();
            displayResultsList.Add(one_result);
        }
    }

    public List<DisplayResults> GetDisplayResultsList()
    {
        return displayResultsList;
    }
}
