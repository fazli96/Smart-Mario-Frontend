using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System;

public class SelectStudentManager : MonoBehaviour
{
    //Singleton
    private static SelectStudentManager instance = null;
    private SceneController scene;
    private List<DisplayResults> displayResultsList;
    public Button exportCSVButton;
    public Button backToTeacherMenuButton;
    public Text CSVerrorMessage;

    public static SelectStudentManager GetSelectStudentManager()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<SelectStudentManager>();
        }
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneController.GetSceneController();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExportCSV()
    {
        UnityEngine.Debug.Log("enter");
        APICall api = APICall.getAPICall();
        StartCoroutine(api.AllStudentResultGetRequest("2")); // to insert studentId from playerprefs**
    }

    public void BackToTeacherMenu()
    {
        scene.ToTeacherMenu();
    }

    public void CSVRetrieved(string result)
    {
        displayResultsList = new List<DisplayResults>();
        JArray data = (JArray)JsonConvert.DeserializeObject(result);
        foreach (JObject one_resultJobj in data)
        {
            DisplayResults one_result = one_resultJobj.ToObject<DisplayResults>();
            displayResultsList.Add(one_result);
        }
        UnityEngine.Debug.Log(displayResultsList[0].difficulty);
        SaveCSV();
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
        try
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
                    CSVerrorMessage.text = "Error! File cannotbe created!";
                }
            }
            StreamWriter writer = new StreamWriter(filePath);

            foreach (DisplayResults result in displayResultsList)
            {
                writer.WriteLine(result.studentId + ", " + result.difficulty + ", " + result.level);
            }

            // writer.WriteLine("11111111111, name, username");
            writer.Flush();
            writer.Close();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log(ex.ToString());
            // Text CSVerrorMessage = GameObject.GetComponent<Text>
            this.CSVerrorMessage.text = "Error! Please ensure CSV file is closed!";
        }
    }
}
