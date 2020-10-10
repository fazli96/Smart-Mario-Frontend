using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Entity that stores information for Student which is used for conversion to a Json form
/// </summary>
public class Student : User
{
    public string teacher_key;
    /// <summary>
    /// Constructor for Student object
    /// </summary>
    /// <param name="username"></param>
    /// <param name="name"></param>
    /// <param name="password"></param>
    /// <param name="teacher_key"></param>
    public Student(string username, string name, string password, string teacher_key)
    {
        this.username = username;
        this.name = name;
        this.password = password;
        this.teacher_key = teacher_key;
    }
}
