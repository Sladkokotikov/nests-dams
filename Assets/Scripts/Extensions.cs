using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class Extensions
{
    public static TOut Mutate<TIn, TOut>(this TIn source, Func<TIn, TOut> mutation)
        => mutation(source);

    public static T With<T>(this T source, Action<T> mutation)
    {
        mutation(source);
        return source;
    }

    public static T Choose<T>(this IEnumerable<T> source)
    {
        var list = source.ToList();
        return list[Random.Range(0, list.Count)];
    }

    public static void Clear(this Transform source)
    {
        foreach(Transform child in source)
            Object.Destroy(child.gameObject);
    }

    public static void Clear(this GameObject source) => source.transform.Clear();

}