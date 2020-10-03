using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student 
{
    public string username;
    public string password;
    public string teacher_key;

    public Student(string username, string password, string teacher_key)
    {
        this.username = username;
        this.password = password;
        this.teacher_key = teacher_key;
    }
}
