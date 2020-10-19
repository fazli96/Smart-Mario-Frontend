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
    private static List<DisplayResults> displayResultsList;
    private static string CSVRawData;
    public Button exportCSVButton;
    public Button backToTeacherMenuButton;
    public Text CSVErrorMessage;
    public Text successMessage;

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


    public void CSVGetRequest(string result)
    {
        CSVRawData = result;
    }

    public void ExportCSV()
    {
        successMessage.text = "";
        CSVErrorMessage.text = "";

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

            JArray data = (JArray)JsonConvert.DeserializeObject(CSVRawData);
            writer.WriteLine("Student Name, " + "Game, " + "Difficulty, " + "Level" + ", " + "Score");
            foreach (JObject one_result in data)
            {
                string studentName = one_result["student"]["name"].ToString();
                string minigameName = one_result["minigame"]["name"].ToString();
                string difficulty = one_result["difficulty"].ToString();
                string level = one_result["level"].ToString();
                // string score = one_result["score"].ToString();
                string score = "100";
                writer.WriteLine(studentName + ", " + minigameName + ", " + difficulty + ", " + level + "," + score);
            }

            writer.Flush();
            writer.Close();

            successMessage.text = "Successfully exported to CSV!";
        }
        catch (Exception ex)
        {
            CSVErrorMessage.text = "Unable to Export! CSV file is in use!";
        }
    }
}
