using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AssignedTasksManager
{
    //Singleton
    private static AssignedTasksManager instance = null;
    
    private static List<StudentTaskCell> studentList;

    public static AssignedTasksManager GetAssignedTasksManager()
    {
        if (instance == null)
        {
            instance = new AssignedTasksManager();
        }
        return instance;
    }

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

    public List<StudentTaskCell> GetStudentList()
    {
        return studentList;
    }
}
