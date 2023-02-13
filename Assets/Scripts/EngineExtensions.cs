using System;
using System.Collections.Generic;
using System.Linq;
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

    public static BasisType Type(this BytecodeBasis basis)
        => basis switch
        {
            BytecodeBasis.Spawn => BasisType.Declaration,
            BytecodeBasis.Kill => BasisType.Declaration,
            BytecodeBasis.Push => BasisType.Declaration,
            BytecodeBasis.Pull => BasisType.Declaration,
            BytecodeBasis.Draw => BasisType.Declaration,
            BytecodeBasis.Lock => BasisType.Declaration,
            BytecodeBasis.Unlock => BasisType.Declaration,
            BytecodeBasis.Break => BasisType.Declaration,
            BytecodeBasis.Build => BasisType.Declaration,

            BytecodeBasis.Adjacent => BasisType.Specification,
            BytecodeBasis.OuterEdge => BasisType.Specification,
            BytecodeBasis.Surrounding => BasisType.Specification,
            BytecodeBasis.Plus => BasisType.Specification,
            BytecodeBasis.Edge => BasisType.Specification,
            // BytecodeBasis.Free => BasisType.Specification,
            // BytecodeBasis.Occupied => BasisType.Specification,
            BytecodeBasis.Beaver => BasisType.Specification,
            BytecodeBasis.Magpie => BasisType.Specification,
            BytecodeBasis.Obstacle => BasisType.Specification,
            BytecodeBasis.Playable => BasisType.Specification,
            BytecodeBasis.None => BasisType.Specification,

            BytecodeBasis.PlayerDeck => BasisType.Specification,
            BytecodeBasis.PlayerGraveyard => BasisType.Specification,
            BytecodeBasis.OpponentDeck => BasisType.Specification,
            BytecodeBasis.OpponentGraveyard => BasisType.Specification,
            BytecodeBasis.OpponentHand => BasisType.Specification,

            BytecodeBasis.Confirm => BasisType.Application,
            BytecodeBasis.ConfirmRandom => BasisType.Application,
            BytecodeBasis.ConfirmAuto => BasisType.Application,

            BytecodeBasis.BeaverCub => BasisType.Specification,

            _ => throw new ArgumentOutOfRangeException(nameof(basis), basis, null)
        };

    public static SpecificationType Argument(this BytecodeBasis basis)
        => basis switch
        {
            BytecodeBasis.Adjacent => SpecificationType.FieldSpecification,
            BytecodeBasis.Surrounding => SpecificationType.FieldSpecification,
            BytecodeBasis.Plus => SpecificationType.FieldSpecification,
            BytecodeBasis.Edge => SpecificationType.FieldSpecification,
            BytecodeBasis.OuterEdge => SpecificationType.FieldSpecification,

            BytecodeBasis.Beaver => SpecificationType.Tribe,
            BytecodeBasis.Magpie => SpecificationType.Tribe,
            BytecodeBasis.Obstacle => SpecificationType.Tribe,
            BytecodeBasis.Playable => SpecificationType.Tribe,
            BytecodeBasis.None => SpecificationType.Tribe,

            BytecodeBasis.PlayerDeck => SpecificationType.CardSource,
            BytecodeBasis.PlayerGraveyard => SpecificationType.CardSource,
            BytecodeBasis.OpponentDeck => SpecificationType.CardSource,
            BytecodeBasis.OpponentGraveyard => SpecificationType.CardSource,
            BytecodeBasis.OpponentHand => SpecificationType.CardSource,

            BytecodeBasis.BeaverCub => SpecificationType.ConcreteCard,

            _ => throw new ArgumentOutOfRangeException(
                $"No ArgumentType for {basis}. Maybe it's not argument at all, maybe not implemented yet")
        };

    public static IEnumerable<Vector2Int> GetSatisfying(this FieldSpecification spec,
        IEnumerable<Vector2Int> possibleTiles, Vector2Int playedPosition,
        Dictionary<Vector2Int, TileInfo> field)
    {
        return spec switch
        {
            FieldSpecification.OuterEdge => field.Keys.SelectMany(Adjacent).Where(t => !field.ContainsKey(t)),
            FieldSpecification.Adjacent => possibleTiles.Where(t => t.IsAdjacent(playedPosition)),
            FieldSpecification.Surrounding => possibleTiles.Where(t => t.IsSurrounding(playedPosition)),
            FieldSpecification.Plus => possibleTiles.Where(t => t.IsOnPlus(playedPosition)),
            FieldSpecification.Edge => possibleTiles.Where(t => t.IsOnEdge(field)),
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
}