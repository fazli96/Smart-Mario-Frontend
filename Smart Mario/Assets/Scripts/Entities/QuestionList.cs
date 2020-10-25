using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is used to store the all the questions retrieved from the database upon login
/// </summary>
public static class QuestionList
{
    private static List<Question> mcqTheoryQuestionListEasy = new List<Question>();
    private static List<Question> mcqTheoryQuestionListMedium = new List<Question>();
    private static List<Question> mcqTheoryQuestionListHard = new List<Question>();
    private static List<Question> mcqCodeQuestionListEasy = new List<Question>();
    private static List<Question> mcqCodeQuestionListMedium = new List<Question>();
    private static List<Question> mcqCodeQuestionListHard = new List<Question>();
    private static List<Question> shortTheoryQuestionListEasy = new List<Question>();
    private static List<Question> shortTheoryQuestionListMedium = new List<Question>();
    private static List<Question> shortTheoryQuestionListHard = new List<Question>();
    private static List<Question> shortCodeQuestionListEasy = new List<Question>();
    private static List<Question> shortCodeQuestionListMedium = new List<Question>();
    private static List<Question> shortCodeQuestionListHard = new List<Question>();

    #region AddQuestions
    /// <summary>
    /// This method is to add a question to existing mcq theory questions for easy difficulty
    /// </summary>
    /// <returns></returns>
    public static void AddMcqTheoryQuestionEasy(Question question)
    {
        mcqTheoryQuestionListEasy.Add(question);
    }
    /// <summary>
    /// This method is to add a question to existing mcq theory questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static void AddMcqTheoryQuestionMedium(Question question)
    {
        mcqTheoryQuestionListMedium.Add(question);
    }
    /// <summary>
    /// This method is to get the mcq theory questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static void AddMcqTheoryQuestionHard(Question question)
    {
        mcqTheoryQuestionListHard.Add(question);
    }

    /// <summary>
    /// This method is to add a question to existing mcq code questions for easy difficulty
    /// </summary>
    /// <returns></returns>
    public static void AddMcqCodeQuestionEasy(Question question)
    {
        mcqCodeQuestionListEasy.Add(question);
    }
    /// <summary>
    /// This method is to add a question to existing mcq code questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static void AddMcqCodeQuestionMedium(Question question)
    {
        mcqCodeQuestionListMedium.Add(question);
    }
    /// <summary>
    /// This method is to add a question to existing mcq code questions for hard difficulty
    /// </summary>
    /// <returns></returns>
    public static void AddMcqCodeQuestionHard(Question question)
    {
        mcqCodeQuestionListHard.Add(question);
    }

    /// <summary>
    /// This method is to add a question to existing short theory questions for easy difficulty
    /// </summary>
    /// <returns></returns>
    public static void AddShortTheoryQuestionEasy(Question question)
    {
        shortTheoryQuestionListEasy.Add(question);
    }
    /// <summary>
    /// This method is to add a question to existing short theory questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static void AddShortTheoryQuestionMedium(Question question)
    {
        shortTheoryQuestionListMedium.Add(question);
    }
    /// <summary>
    /// This method is to add a question to existing short theory questions for hard difficulty
    /// </summary>
    /// <returns></returns>
    public static void AddShortTheoryQuestionHard(Question question)
    {
        shortTheoryQuestionListHard.Add(question);
    }

    /// <summary>
    /// This method is to add a question to existing short code questions for easy difficulty
    /// </summary>
    /// <returns></returns>
    public static void AddShortCodeQuestionEasy(Question question)
    {
        shortCodeQuestionListEasy.Add(question);
    }
    /// <summary>
    /// This method is to add a question to existing short code questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static void AddShortCodeQuestionMedium(Question question)
    {
        shortCodeQuestionListMedium.Add(question);
    }
    /// <summary>
    /// This method is to add a question to existing short code questions for hard difficulty
    /// </summary>
    /// <returns></returns>
    public static void AddShortCodeQuestionHard(Question question)
    {
        shortCodeQuestionListHard.Add(question);
    }
    #endregion

    #region clearQuestionList
    /// <summary>
    /// This method is to clear the list of the mcq theory questions for easy difficulty
    /// </summary>
    /// <returns></returns>
    public static void ClearMcqTheoryQuestionListEasy()
    {
        mcqTheoryQuestionListEasy.Clear();
    }
    /// <summary>
    /// This method is to clear the list of the mcq theory questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static void ClearMcqTheoryQuestionListMedium()
    {
        mcqTheoryQuestionListMedium.Clear();
    }
    /// <summary>
    /// This method is to clear the list of mcq theory questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static void ClearMcqTheoryQuestionListHard()
    {
        mcqTheoryQuestionListHard.Clear();
    }

    /// <summary>
    /// This method is to clear the list of mcq code questions for easy difficulty
    /// </summary>
    /// <returns></returns>
    public static void ClearMcqCodeQuestionListEasy()
    {
        mcqCodeQuestionListEasy.Clear();
    }
    /// <summary>
    /// This method is to clear the list of mcq code questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static void ClearMcqCodeQuestionListMedium()
    {
        mcqCodeQuestionListMedium.Clear();
    }
    /// <summary>
    /// This method is to clear the list of mcq code questions for hard difficulty
    /// </summary>
    /// <returns></returns>
    public static void ClearMcqCodeQuestionListHard()
    {
        mcqCodeQuestionListHard.Clear();
    }

    /// <summary>
    /// This method is to clear the list of short theory questions for easy difficulty
    /// </summary>
    /// <returns></returns>
    public static void ClearShortTheoryQuestionListEasy()
    {
        shortTheoryQuestionListEasy.Clear();
    }
    /// <summary>
    /// This method is to clear the list of short theory questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static void ClearShortTheoryQuestionListMedium()
    {
        shortTheoryQuestionListMedium.Clear();
    }
    /// <summary>
    /// This method is to clear the list of short theory questions for hard difficulty
    /// </summary>
    /// <returns></returns>
    public static void ClearShortTheoryQuestionListHard()
    {
        shortTheoryQuestionListHard.Clear();
    }

    /// <summary>
    /// This method is to clear the list of short code questions for easy difficulty
    /// </summary>
    /// <returns></returns>
    public static void ClearShortCodeQuestionListEasy()
    {
        shortCodeQuestionListEasy.Clear();
    }
    /// <summary>
    /// This method is to clear the list of short code questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static void ClearShortCodeQuestionListMedium()
    {
        shortCodeQuestionListMedium.Clear();
    }
    /// <summary>
    /// This method is to clear the list of short code questions for hard difficulty
    /// </summary>
    /// <returns></returns>
    public static void ClearShortCodeQuestionListHard()
    {
        shortCodeQuestionListHard.Clear();
    }
    #endregion

    #region GetMethods
    /// <summary>
    /// This method is to get the mcq theory questions for easy difficulty
    /// </summary>
    /// <returns></returns>
    public static List<Question> GetMcqTheoryQuestionListEasy()
    {
        List<Question> newQuestionList = new List<Question>();
        for (int i = 0; i < mcqTheoryQuestionListEasy.Count; i++)
        {
            newQuestionList.Add(mcqTheoryQuestionListEasy[i]);
        }
        ShuffleList.Shuffle(newQuestionList);
        return newQuestionList;
    }
    /// <summary>
    /// This method is to get the mcq theory questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static List<Question> GetMcqTheoryQuestionListMedium()
    {
        List<Question> newQuestionList = new List<Question>();
        for (int i = 0; i < mcqTheoryQuestionListMedium.Count; i++)
        {
            newQuestionList.Add(mcqTheoryQuestionListMedium[i]);
        }
        ShuffleList.Shuffle(newQuestionList);
        return newQuestionList;
    }
    /// <summary>
    /// This method is to get the mcq theory questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static List<Question> GetMcqTheoryQuestionListHard()
    {
        List<Question> newQuestionList = new List<Question>();
        for (int i = 0; i < mcqTheoryQuestionListHard.Count; i++)
        {
            newQuestionList.Add(mcqTheoryQuestionListHard[i]);
        }
        ShuffleList.Shuffle(newQuestionList);
        return newQuestionList;
    }

    /// <summary>
    /// This method is to get the mcq code questions for easy difficulty
    /// </summary>
    /// <returns></returns>
    public static List<Question> GetMcqCodeQuestionListEasy()
    {
        List<Question> newQuestionList = new List<Question>();
        for (int i = 0; i < mcqCodeQuestionListEasy.Count; i++)
        {
            newQuestionList.Add(mcqCodeQuestionListEasy[i]);
        }
        ShuffleList.Shuffle(newQuestionList);
        return newQuestionList;
    }
    /// <summary>
    /// This method is to get the mcq code questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static List<Question> GetMcqCodeQuestionListMedium()
    {
        List<Question> newQuestionList = new List<Question>();
        for (int i = 0; i < mcqCodeQuestionListMedium.Count; i++)
        {
            newQuestionList.Add(mcqCodeQuestionListMedium[i]);
        }
        ShuffleList.Shuffle(newQuestionList);
        return newQuestionList;
    }
    /// <summary>
    /// This method is to get the mcq code questions for hard difficulty
    /// </summary>
    /// <returns></returns>
    public static List<Question> GetMcqCodeQuestionListHard()
    {
        List<Question> newQuestionList = new List<Question>();
        for (int i = 0; i < mcqCodeQuestionListHard.Count; i++)
        {
            newQuestionList.Add(mcqCodeQuestionListHard[i]);
        }
        ShuffleList.Shuffle(newQuestionList);
        return newQuestionList;
    }

    /// <summary>
    /// This method is to get the short theory questions for easy difficulty
    /// </summary>
    /// <returns></returns>
    public static List<Question> GetShortTheoryQuestionListEasy()
    {
        List<Question> newQuestionList = new List<Question>();
        for (int i = 0; i < shortTheoryQuestionListEasy.Count; i++)
        {
            newQuestionList.Add(shortTheoryQuestionListEasy[i]);
        }
        ShuffleList.Shuffle(newQuestionList);
        return newQuestionList;
    }
    /// <summary>
    /// This method is to get the short theory questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static List<Question> GetShortTheoryQuestionListMedium()
    {
        List<Question> newQuestionList = new List<Question>();
        for (int i = 0; i < shortTheoryQuestionListMedium.Count; i++)
        {
            newQuestionList.Add(shortTheoryQuestionListMedium[i]);
        }
        ShuffleList.Shuffle(newQuestionList);
        return newQuestionList;
    }
    /// <summary>
    /// This method is to get the short theory questions for hard difficulty
    /// </summary>
    /// <returns></returns>
    public static List<Question> GetShortTheoryQuestionListHard()
    {
        List<Question> newQuestionList = new List<Question>();
        for (int i = 0; i < shortTheoryQuestionListHard.Count; i++)
        {
            newQuestionList.Add(shortTheoryQuestionListHard[i]);
        }
        ShuffleList.Shuffle(newQuestionList);
        return newQuestionList;
    }

    /// <summary>
    /// This method is to get the short code questions for easy difficulty
    /// </summary>
    /// <returns></returns>
    public static List<Question> GetShortCodeQuestionListEasy()
    {
        List<Question> newQuestionList = new List<Question>();
        for (int i = 0; i < shortCodeQuestionListEasy.Count; i++)
        {
            newQuestionList.Add(shortCodeQuestionListEasy[i]);
        }
        ShuffleList.Shuffle(newQuestionList);
        return newQuestionList;
    }
    /// <summary>
    /// This method is to get the short code questions for medium difficulty
    /// </summary>
    /// <returns></returns>
    public static List<Question> GetShortCodeQuestionListMedium()
    {
        List<Question> newQuestionList = new List<Question>();
        for (int i = 0; i < shortCodeQuestionListMedium.Count; i++)
        {
            newQuestionList.Add(shortCodeQuestionListMedium[i]);
        }
        ShuffleList.Shuffle(newQuestionList);
        return newQuestionList;
    }
    /// <summary>
    /// This method is to get the short code questions for hard difficulty
    /// </summary>
    /// <returns></returns>
    public static List<Question> GetShortCodeQuestionListHard()
    {
        List<Question> newQuestionList = new List<Question>();
        for (int i = 0; i < shortCodeQuestionListHard.Count; i++)
        {
            newQuestionList.Add(shortCodeQuestionListHard[i]);
        }
        ShuffleList.Shuffle(newQuestionList);
        return newQuestionList;
    }
    #endregion

}
