using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Entity for storing student's character attributes like studentId and the character chosen
/// </summary>
public class CustomCharacter 
{
    public string studentId;
    public string custom;
    /// <summary>
    /// Constructor for CustomCharacter object
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="custom"></param>
    public CustomCharacter(string studentId, string custom)
    {
        this.studentId = studentId;
        this.custom = custom;
    }
}
