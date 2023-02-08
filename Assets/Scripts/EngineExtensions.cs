using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EngineExtensions
{
    public static bool IsOnEdge<T>(this Vector2Int source, Dictionary<Vector2Int,T> field)
        => Adjacent(source).Any(p => !field.ContainsKey(p));

    public static bool IsOnPlus(this Vector2Int source, Vector2Int other)
    {
        var abs = Vector2Int.Max(source - other, other - source);
        return abs.x * abs.y == 0 && abs.x + abs.y > 0;
    }

    public static bool IsSurrounding(this Vector2Int source, Vector2Int other)
        => Surrounding(source).Any(v => v == other);

    public static IEnumerable<Vector2Int> Surrounding(this Vector2Int source)
    {
        yield return source + Vector2Int.up;
        yield return source + Vector2Int.up + Vector2Int.right;
        yield return source + Vector2Int.right;
        yield return source + Vector2Int.right + Vector2Int.down;
        yield return source + Vector2Int.down;
        yield return source + Vector2Int.down + Vector2Int.left;
        yield return source + Vector2Int.left;
        yield return source + Vector2Int.left + Vector2Int.up;
    }

    public static IEnumerable<Vector2Int> Adjacent(this Vector2Int source)
    {
        yield return source + Vector2Int.up;
        yield return source + Vector2Int.right;
        yield return source + Vector2Int.down;
        yield return source + Vector2Int.left;
    }

    public static bool IsAdjacent(this Vector2Int source, Vector2Int other)
        => Adjacent(source).Any(v => v == other);

}