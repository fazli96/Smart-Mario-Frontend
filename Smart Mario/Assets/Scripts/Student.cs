using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student 
{
    public string username;
    public string password;
    public string student_key;

    public Student(string username, string password, string student_key)
    {
        this.username = username;
        this.password = password;
        this.student_key = student_key;
    }
}
