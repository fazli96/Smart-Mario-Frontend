using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// A class which stores the list of items after retriving from the API call. These items will be retrieved and displayed to the user,
/// </summary>
public class DisplayListManager
{
    //Singleton
    private static DisplayListManager instance = null;
    
    private List<DisplayResults> displayResultsList;
    
    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static DisplayListManager GetDisplayListManager()
    {
        if (instance == null)
        {
            instance = new DisplayListManager();
        }
        return instance;
    }

    /// <summary>
    /// Function Call for the API to store the data in this manager
    /// </summary>
    /// <param name="result"></param>
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

    /// <summary>
    /// Function call for the displaying manager to retrieve the stored results
    /// </summary>
    /// <returns></returns>
    public List<DisplayResults> GetDisplayResultsList()
    {
        return displayResultsList;
    }
}
