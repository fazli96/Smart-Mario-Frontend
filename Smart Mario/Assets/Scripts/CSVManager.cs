using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;

public class CSVManager : MonoBehaviour
{   //Singleton
    private static CSVManager instance = null;

    public static CSVManager GetCSVManager()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<CSVManager>();
        }
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CSVRetrieved(string result)
    {
        UnityEngine.Debug.Log(result);
        JArray data = (JArray)JsonConvert.DeserializeObject(result);
        foreach (JObject one_result in data)
        {
            Debug.Log(one_result);
        }
        /*JArray data2 = data["allQuestions"].Value<JArray>();
        foreach (JObject questionObject in data2)
        {
            Debug.Log("question: " + questionObject);
            Question question1 = questionObject.ToObject<Question>();
            Debug.Log(question1.option1);
          //  questionList.Add(question1);
        }
       // Debug.Log("DBResult: " + result);
       // Debug.Log("allQuestions" + data2);*/
    }
        
    private string getPath()
    {
    #if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + "Student_Statistics.csv";
    #else
        return Application.dataPath + "/" + "Student_Statistics.csv";
    #endif
    }

    public void SaveCSV()
    {
        string filePath = getPath();
        if (!File.Exists(filePath))
        {
            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(filePath, 1024))
                {
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.ToString());
            }
        }
        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine("11111111111, name, username");
        writer.Flush();
        writer.Close();
    }
}


