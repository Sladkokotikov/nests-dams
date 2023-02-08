using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Goal
{
    private readonly HashSet<Vector2Int> _positions;
    private Vector2Int KeyPoint { get; set; }
    public Vector2Int RightUp { get; }
    public Tribe Tribe { get; }

    public IEnumerable<Vector2Int> Positions()
    {
        foreach (var pos in _positions)
            yield return pos + KeyPoint;
        yield return KeyPoint;
    }

    public Goal(Tribe tribe, params Vector2Int[] relativePositions)
    {
        Tribe = tribe;
        _positions = new HashSet<Vector2Int>();
        RightUp = new Vector2Int();
        foreach (var pos in relativePositions)
        {
            _positions.Add(pos);
            RightUp = Vector2Int.Max(RightUp, pos);
        }
    }

    public bool IsSatisfied(Dictionary<Vector2Int, TileInfo> field)
    {
        var rightTribe = new HashSet<Vector2Int>();
        foreach (var pair in field.Where(pair => pair.Value.OccupantTribe == Tribe))
            rightTribe.Add(pair.Key);
        try
        {
            KeyPoint = rightTribe.First(pos => _positions.All(delta => rightTribe.Contains(pos + delta)));
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }
}