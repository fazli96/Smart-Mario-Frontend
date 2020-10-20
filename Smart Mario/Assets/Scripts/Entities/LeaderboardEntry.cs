using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Entity for storing student's username, id and total score for leaderboard
/// </summary>
public class LeaderboardEntry
{
    /// <summary>
    /// This is the get and set method for question Id
    /// </summary>
    [JsonProperty("total_score")]
    public string score { get; set; }

    /// <summary>
    /// This is the get and set method for question Title
    /// </summary>
    [JsonProperty("student.id")]
    public string id { get; set; }

    /// <summary>
    /// This is the get and set method for option 1
    /// </summary>
    [JsonProperty("student.username")]
    public string name { get; set; }
}
