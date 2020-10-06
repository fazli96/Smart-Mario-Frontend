using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : User
{
    public string teacher_key;

    public Student(string username, string name, string password, string teacher_key)
    {
        this.username = username;
        this.name = name;
        this.password = password;
        this.teacher_key = teacher_key;
    }
}
