using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// This class contains methods to shuffle a collection of strongly typed objects
/// </summary>
public static class ShuffleList
{
    /// <summary>
    /// This method is to shuffle a collection of strongly typed objects randomly
    /// </summary>
    /// <typeparam name="any type"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<T> Shuffle<T>(List<T> list)
    {
        Random rnd = new Random();
        for (int i = 0; i < list.Count; i++)
        {
            int k = rnd.Next(0, i);
            T value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
        return list;
    }
}
