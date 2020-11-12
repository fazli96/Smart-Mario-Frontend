using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which translates translate between game console requirements and what is stored in the database
/// </summary>
public class Translator : MonoBehaviour
{
    //Singleton
    private static Translator instance = null;

    /// <summary>
    /// Creates a singleton instance if none exist, returns the existing instance if one exists
    /// </summary>
    /// <returns></returns>
    public static Translator GetTranslator()
    {
        if (instance == null)
        {
            instance = new Translator();
        }
        return instance;
    }
    
    /// <summary>
    /// Dictionary for forward translation
    /// </summary>
    /// <typeparam name="string"></typeparam>
    /// <typeparam name="string"></typeparam>
    /// <returns></returns>
    private static Dictionary<string, string> GameNames = new Dictionary<string, string>()
    {
        {"1", "World 1 Stranded"},
        {"2", "World 1 Card Matching"},
        {"3", "World 2 Stranded"},
        {"4", "World 2 Card Matching"}
    };

    /// <summary>
    /// Dictionary for backward translation
    /// </summary>
    /// <typeparam name="string"></typeparam>
    /// <typeparam name="string"></typeparam>
    /// <returns></returns>
    private static Dictionary<string, string> GameIds = new Dictionary<string, string>()
    {
        {"World 1 Stranded", "1"},
        {"World 1 Card Matching", "2"},
        {"World 2 Stranded", "3"},
        {"World 2 Card Matching", "4"}
    };

    /// <summary>
    /// Translate from gameId to gameName
    /// </summary>
    /// <param name="gameId"></param>
    /// <returns></returns>
    public string GameIDToName(string gameId) { return GameNames[gameId]; }

    /// <summary>
    /// Translate from GameName to GameID
    /// </summary>
    /// <param name="gameName"></param>
    /// <returns></returns>
    public string GameNameToID(string gameName) { return GameIds[gameName]; }

    /// <summary>
    /// Adds the "Level " string before the integer value of the number
    /// </summary>
    /// <param name="levelstr"></param>
    /// <returns></returns>
    public int LevelStringToInt(string levelstr)
    {
        char level = levelstr[levelstr.Length -1];
        return (int) Char.GetNumericValue(level);
    }

}
