using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    [JsonProperty("ID")]
    public string Id { get; set; }
    [JsonProperty("Question")]
    public string questionTitle { get; set; }
    [JsonProperty("1")]
    public string option1 { get; set; }
    [JsonProperty("2")]
    public string option2 { get; set; }
    [JsonProperty("3")]
    public string option3 { get; set; }
    [JsonProperty("4")]
    public string option4 { get; set; }
    [JsonProperty("Answer")]
    public string answer { get; set; }
    [JsonProperty("Explanation")]
    public string explanation { get; set; }
}