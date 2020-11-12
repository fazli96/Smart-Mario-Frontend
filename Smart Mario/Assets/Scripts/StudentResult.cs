using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which stores the attributes of a student's results
/// </summary>
public class StudentResult 
{
    public int id;
    public string username;
    public string name;
    public string password;
    public string createdAt;
    public string updatedAt;
    public int fk_teacher_key;
}
