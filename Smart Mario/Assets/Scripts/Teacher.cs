using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Teacher
{
    public string username;
    public string password;
    public string school_key;

    public Teacher (string username, string password, string school_key)
    {
        this.username = username;
        this.password = password;
        this.school_key = school_key;
    }

}
