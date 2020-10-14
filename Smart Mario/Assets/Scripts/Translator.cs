using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator : MonoBehaviour
{
    //Singleton
    private static Translator instance = null;

    public static Translator GetTranslator()
    {
        if (instance == null)
        {
            instance = new Translator();
        }
        return instance;
    }
    
    private static Dictionary<int, string> GameNames = new Dictionary<int, string>()
    {
        {1, "World 1 Stranded"},
        {2, "World 1 Card Matching"},
        {3, "World 2 Stranded"},
        {4, "World 2 Card Matching"}
    };

    private static Dictionary<string, int> GameIds = new Dictionary<string, int>()
    {
        {"World 1 Stranded", 1},
        {"World 1 Card Matching", 2},
        {"World 2 Stranded", 3},
        {"World 2 Card Matching", 4}
    };

    public string GameIDToName(int gameId) { return GameNames[gameId]; }

    public int GameNameToID(string gameName) { return GameIds[gameName]; }

    public int LevelStringToInt(string levelstr)
    {
        char level = levelstr[levelstr.Length -1];
        return (int) Char.GetNumericValue(level);
    }

}
