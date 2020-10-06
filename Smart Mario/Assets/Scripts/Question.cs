using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Entity for question storing attributes for ID, Question text, MCQ options text, Answer and Explanation
/// </summary>
public class Question
{
    /// <summary>
    /// This is the get and set method for question Id
    /// </summary>
    [JsonProperty("ID")]
    public string Id { get; set; }

    /// <summary>
    /// This is the get and set method for question Title
    /// </summary>
    [JsonProperty("Question")]
    public string questionTitle { get; set; }

    /// <summary>
    /// This is the get and set method for option 1
    /// </summary>
    [JsonProperty("1")]
    public string option1 { get; set; }

    /// <summary>
    /// This is the get and set method for option 2
    /// </summary>
    [JsonProperty("2")]
    public string option2 { get; set; }

    /// <summary>
    /// This is the get and set method for option 3
    /// </summary>
    [JsonProperty("3")]
    public string option3 { get; set; }

    /// <summary>
    /// This is the get and set method for option 4
    /// </summary>
    [JsonProperty("4")]
    public string option4 { get; set; }

    /// <summary>
    /// This is the get and set method for the correct answer
    /// </summary>
    [JsonProperty("Answer")]
    public string answer { get; set; }

    /// <summary>
    /// This is the get and set method for the explanation of the correct answer
    /// </summary>
    [JsonProperty("Explanation")]
    public string explanation { get; set; }
}