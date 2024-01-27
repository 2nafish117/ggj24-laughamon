using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static T GetRandom<T>(this List<T> list, out int index)
    {
        index = Random.Range(0, list.Count);
        return list[index];
    }

    public static T GetRandom<T>(this T[] array, out int index)
    {
        index = Random.Range(0, array.Length);
        return array[index];
    }

    public static void RemoveAtSwapBack<T>(this List<T> list, int index)
    {
        int last = list.Count - 1;
        list[index] = list[last];
        list.RemoveAt(last);
    }
}
