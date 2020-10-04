using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher : User
{
    public string school_key;

    public Teacher (string username, string name, string password, string school_key)
    {
        this.username = username;
        this.name = name;
        this.password = password;
        this.school_key = school_key;
    }

}
