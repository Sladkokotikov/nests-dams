using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class Extensions
{
    public static T With<T>(this T source, Action<T> mutation)
    {
        mutation(source);
        return source;
    }

    public static T With<T>(this T source, Action<T> mutation, bool condition)
    {
        if (condition)
            mutation(source);
        return source;
    }

    public static T Choose<T>(this IEnumerable<T> source)
    {
        var list = source.ToList();
        return list[Random.Range(0, list.Count)];
    }

    public static IEnumerable<T> Shuffled<T>(this IEnumerable<T> source)
    {
        var array = source.ToArray();
        var indices = Enumerable.Range(0, array.Length).ToList();
        foreach (var _ in array)
        {
            var index = Random.Range(0, indices.Count);
            yield return array[indices[index]];
            indices.RemoveAt(index);
        }
    }

    public static void Clear(this Transform source)
    {
        foreach (Transform child in source)
            Object.Destroy(child.gameObject);
    }

    public static void Clear(this GameObject source) => source.transform.Clear();

    public static T[] Enumerate<T>() => (T[]) Enum.GetValues(typeof(T));
}