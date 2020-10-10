using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Entity that stores information for Teacher which is used for conversion to a Json form
/// </summary>
public class Teacher : User
{
    public string school_key;
    /// <summary>
    /// Constructor for Student object
    /// </summary>
    /// <param name="username"></param>
    /// <param name="name"></param>
    /// <param name="password"></param>
    /// <param name="school_key"></param>
    public Teacher (string username, string name, string password, string school_key)
    {
        this.username = username;
        this.name = name;
        this.password = password;
        this.school_key = school_key;
    }

}
