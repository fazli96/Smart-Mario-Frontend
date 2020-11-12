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

/// <summary>
/// Controller class for the display of the scene to select a student to view their performance
/// </summary>
public class SelectStudentManager : MonoBehaviour
{
    //Singleton
    private static SelectStudentManager instance = null;

    private SceneController scene;
    private static List<DisplayResults> displayResultsList;
    public Button exportCSVButton;
    public Button backToTeacherMenuButton;
    public Text CSVErrorMessage;
    public Text successMessage;
    public Text processingCSVMessage;

    private static string teacherId;
    private static bool refreshAndExportCSV;
    private static string CSVRawData;

    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static SelectStudentManager GetSelectStudentManager()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            instance = go.AddComponent<SelectStudentManager>();
        }
        return instance;
    }

    /// <summary>
    /// Starts the scene and clears all messages
    /// If exportcsv is called, scene refreshes and exports the csv
    /// </summary>
    void Start()
    {
        scene = SceneController.GetSceneController();
        teacherId = PlayerPrefs.GetString("teacherId");

        successMessage.text = "";
        CSVErrorMessage.text = "";
        processingCSVMessage.text = "";
        if (refreshAndExportCSV)
        {
            ProcessCSVResponse();
        }
        refreshAndExportCSV = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Back action button
    /// Directs user back to the main menu
    /// </summary>
    public void BackToTeacherMenu()
    {
        scene.ToTeacherMenu();
    }

    /// <summary>
    /// Gets the path to export csv
    /// </summary>
    /// <returns></returns>
    private string getPath()
    {
        #if UNITY_EDITOR
                return Application.dataPath + "/CSV/" + "Student_Statistics.csv";
        #else
                return Application.dataPath + "/" + "Student_Statistics.csv";
        #endif
    }

    /// <summary>
    /// Gets the string for csv export and stores it before refreshing the page
    /// </summary>
    /// <param name="responseString"></param>
    public void SetRefreshAndExportCSV(string responseString)
    {
        refreshAndExportCSV = true;
        CSVRawData = responseString;
    }

    /// <summary>
    /// Exports all student's progress and data
    /// </summary>
    public void ExportCSV()
    {
        successMessage.text = "";
        CSVErrorMessage.text = "";
        processingCSVMessage.text = "Processing CSV, please hold on...";
        APICall apiCall = APICall.getAPICall();
        StartCoroutine(apiCall.CSVExportGetRequest(teacherId));
    }

    /// <summary>
    /// To process the response string into a csv output
    /// </summary>
    public void ProcessCSVResponse()
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

            UnityEngine.Debug.Log(CSVRawData);
            JObject data = (JObject)JsonConvert.DeserializeObject(CSVRawData);
            writer.WriteLine("Student Name, " + "Game, " + "Difficulty, " + "Level" + ", " + "Score");
            UnityEngine.Debug.Log(data);
            foreach (JObject student in data["data"])
            {
                UnityEngine.Debug.Log(student);
                string studentName = student["name"].ToString();
                foreach (JObject result in student["results"])
                {
                    UnityEngine.Debug.Log(result);
                    UnityEngine.Debug.Log("here");
                    string minigameName = result["minigame"]["name"].ToString();
                    string difficulty = result["difficulty"].ToString();
                    string level = result["level"].ToString();
                    string score = result["score"].ToString();
                    writer.WriteLine(studentName + ", " + minigameName + ", " + difficulty + ", " + level + "," + score);
                }    
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
