using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Teacher
{
    public string username;
    public string password;
    public string teacher_key;

    public Teacher (string username, string password, string teacher_key)
    {
        this.username = username;
        this.password = password;
        this.teacher_key = teacher_key;
    }

}
