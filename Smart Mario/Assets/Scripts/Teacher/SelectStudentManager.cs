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
    // private List<CSVDisplay> csvDisplay;
    public Button exportCSVButton;
    public Button backToTeacherMenuButton;
    public Text CSVErrorMessage;

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
        APICall api = APICall.getAPICall();
        StartCoroutine(api.AllStudentResultGetRequest("2")); // to insert studentId from playerprefs**
    }

    public void BackToTeacherMenu()
    {
        scene.ToTeacherMenu();
    }



    private string getPath()
    {
        #if UNITY_EDITOR
                return Application.dataPath + "/CSV/" + "Student_Statistics.csv";
        #else
                return Application.dataPath + "/" + "Student_Statistics.csv";
        #endif
    }

    // public void CSVRetrieved(string result)
    // {
    //     displayResultsList = new List<DisplayResults>();
    //     JArray data = (JArray)JsonConvert.DeserializeObject(result);
    //     foreach (JObject one_resultJobj in data)
    //     {
    //         DisplayResults one_result = one_resultJobj.ToObject<DisplayResults>();
    //         displayResultsList.Add(one_result);
    //     }
    //     SaveCSV();
    // }

    public void CSVRetrieved(string result)
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
                    CSVErrorMessage.text = "Error! File cannot be created!";
                }
            }
            StreamWriter writer = new StreamWriter(filePath);

            JArray data = (JArray)JsonConvert.DeserializeObject(result);
            writer.WriteLine("Student Name, " + "Difficulty, " + "Level" + ", " + "Score");
            foreach (JObject one_result in data)
            {
                UnityEngine.Debug.Log(one_result["student"]["name"]);
                string studentID = one_result["student"]["name"].ToString();
                string difficulty = one_result["difficulty"].ToString();
                string level = one_result["level"].ToString();
                string score = one_result["results"]["score"].ToString();
                writer.WriteLine(studentID + ", " + difficulty + ", " + level + "," + score);
            }
            
            // writer.WriteLine("11111111111, name, username");
            writer.Flush();
            writer.Close();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log(ex.ToString());
            // Text CSVerrorMessage = GameObject.GetComponent<Text>
            this.CSVErrorMessage.text = "Error! Please ensure CSV file is closed!";
        }
    }
}
