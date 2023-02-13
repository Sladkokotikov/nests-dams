using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Enums;
using UnityEngine;

public static class EngineExtensions
{
    public static bool IsOnEdge<T>(this Vector2Int source, Dictionary<Vector2Int, T> field)
        => Adjacent(source).Any(p => !field.ContainsKey(p));

    public static bool IsOnPlus(this Vector2Int source, Vector2Int other)
    {
        var abs = Vector2Int.Max(source - other, other - source);
        return abs.x * abs.y == 0 && abs.x + abs.y > 0;
    }

    public static bool IsSurrounding(this Vector2Int source, Vector2Int other)
        => Surrounding(source).Any(v => v == other);

    private static IEnumerable<Vector2Int> GetPullTargets(Vector2Int playedPosition, Dictionary<Vector2Int, TileInfo> field)
    {
        var lastRaycastDirection = -Vector2Int.one;
        var nextDirectionDelta = Vector2Int.left;
        var result = new List<Vector2Int>();
        for (var i = 0; i < 8; i++)
        {
            if (i % 2 == 0)
                nextDirectionDelta = new Vector2Int(nextDirectionDelta.y, -nextDirectionDelta.x);
            var currPos = playedPosition;
            while (field.ContainsKey(currPos + lastRaycastDirection))
            {
                currPos += lastRaycastDirection;
                if (!field[currPos].Occupied)
                    continue;
                result.Add(currPos);
                break;
            }

            lastRaycastDirection += nextDirectionDelta;
        }

        return result;
    }
    
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

    public static IEnumerable<Vector2Int> GetSatisfying(this FieldSpecificationType spec,
        IEnumerable<Vector2Int> possibleTiles, Vector2Int playedPosition,
        Dictionary<Vector2Int, TileInfo> field)
    {
        return spec switch
        {
            FieldSpecificationType.OuterEdge => field.Keys.SelectMany(Adjacent).Where(t => !field.ContainsKey(t)),
            FieldSpecificationType.Adjacent => possibleTiles.Where(t => t.IsAdjacent(playedPosition)),
            FieldSpecificationType.Surrounding => possibleTiles.Where(t => t.IsSurrounding(playedPosition)),
            FieldSpecificationType.Plus => possibleTiles.Where(t => t.IsOnPlus(playedPosition)),
            FieldSpecificationType.Edge => possibleTiles.Where(t => t.IsOnEdge(field)),
            _ => throw new ArgumentOutOfRangeException(nameof(spec), spec, null)
        };
    }

    public static bool Satisfied(this Tribe source, Tribe other)
    {
        return source switch
        {
            Tribe.Beaver => other == Tribe.Beaver,
            Tribe.Magpie => other == Tribe.Magpie,
            Tribe.Obstacle => other == Tribe.Obstacle,
            Tribe.None => other == Tribe.None,
            Tribe.Playable => other != Tribe.None && other != Tribe.Obstacle,

            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }

    public static BytecodeBasis Bb(this byte b)
        => (BytecodeBasis) b;

    public static T To<T>(this byte b)
        => (T) Enum.Parse(typeof(T), b.Bb().ToString());
}