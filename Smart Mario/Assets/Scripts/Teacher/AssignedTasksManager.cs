using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Controller responsible for retrieving and storing the tasks assigned to the students and their completion status
/// </summary>
public class AssignedTasksManager
{
    //Singleton
    private static AssignedTasksManager instance = null;
    
    private static List<StudentTaskCell> studentList;

    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static AssignedTasksManager GetAssignedTasksManager()
    {
        if (instance == null)
        {
            instance = new AssignedTasksManager();
        }
        return instance;
    }

    /// <summary>
    /// Function call to retrieve the data and store it
    /// </summary>
    /// <param name="str"></param>
    public void RetrieveData(string str)
    {
        studentList = new List<StudentTaskCell>();
        JArray data = (JArray)JsonConvert.DeserializeObject(str);
        int count = 0;
        foreach (JObject one_result in data)
        {
            string studentName = one_result["student"]["name"].ToString();
            string completionStatus = one_result["completed"].ToString();
            studentList.Add(new StudentTaskCell(studentName, completionStatus));
        }
    }

    /// <summary>
    /// Getter function to get the list stored
    /// </summary>
    /// <returns></returns>
    public List<StudentTaskCell> GetStudentList()
    {
        return studentList;
    }
}
