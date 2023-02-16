using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Configuration;
using DG.Tweening;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private CardMovement cardPrefab;
    private Dictionary<Vector2Int, TileAnimation> _field;
    [SerializeField] private RectTransform rect;
    [SerializeField] private TileAnimation tilePrefab;
    [SerializeField] private AnimationConfiguration animConfig;
    [SerializeField] private MatchConfiguration matchConfig;
    [SerializeField] private Game game;

    [field:SerializeField] public RectTransform AboveAll { get; private set; }

    public IEnumerator CreateAndPlaceCard(Player player, CardData card, Vector2Int position, bool spawn)
    {
        Instantiate(cardPrefab, transform)
            .With(c => c.Player = player)
            .With(c => c.Card.Data = card)
            .With(c => PlaceCard(c, position))
            .With(c => c.CardAnimation.VisualizeBirth(), spawn);
        yield break;
    }

    private List<TileAnimation> _temporaryTiles;

    public IEnumerator ShowPossibleTiles(IEnumerable<Vector2Int> selectedPoints)
    {
        _temporaryTiles = new List<TileAnimation>();
        foreach (var point in selectedPoints.Shuffled().ToList())
        {
            var tile = _field.ContainsKey(point)
                ? _field[point]
                : Instantiate(tilePrefab, transform)
                    .With(t => t.Tile.Position = point)
                    .With(t => t.Width = matchConfig.TileWidth)
                    .With(t => t.Tile.Player = game.Engine.Player)
                    .With(t => t.Build(animConfig.TileShowDuration));
            tile.Tile.Possible = true;
            if (!_field.ContainsKey(point))
                _temporaryTiles.Add(tile);
        }

        yield return new WaitForSeconds(animConfig.TileShowDuration);
    }

    public IEnumerator Show(Dictionary<Vector2Int, TileInfo> field)
    {
        _field = new Dictionary<Vector2Int, TileAnimation>();
        foreach (var position in field.Keys.Shuffled())
        {
            yield return Build(position);
            yield return new WaitForSeconds(0.08f);
        }
    }

    public IEnumerator HidePossibleTiles(IEnumerable<Vector2Int> possibleTiles)
    {
        foreach (var tile in possibleTiles.Where(p => _field.ContainsKey(p)))
            _field[tile].Tile.Possible = false;

        foreach (var tile in _temporaryTiles)
            tile.Break(animConfig.TileHideDuration);

        yield return new WaitForSeconds(animConfig.TileHideDuration);
        _temporaryTiles = new List<TileAnimation>();
    }

    public void Tilt() => rect.anchoredPosition +=
        matchConfig.TileWidth / 2f * new Vector2(
            1 - matchConfig.FieldSize.x % 2,
            1 - matchConfig.FieldSize.y % 2);

    public void Win(Goal goal)
    {
        foreach (var tile in goal.Positions())
            _field[tile].Highlight();
    }

    private bool CanPlaceCard(Vector2Int pos)
        => _field.ContainsKey(pos) && _field[pos].Tile.Free;

    public Vector2Int? PlacePosition(Vector3 cardPosition)
    {
        var realPos = cardPosition - rect.position;
        var pos = Vector2Int.RoundToInt(realPos / matchConfig.TileWidth);
        if (CanPlaceCard(pos))
            return pos;
        return null;
    }

    public void PlaceCard(CardMovement card, Vector2Int position)
    {
        card.ToField(position);
        _field[position].Tile.Occupant = card;
        card.Parent = _field[position];
    }

    public IEnumerator Build(Vector2Int position)
    {
        _field[position] = Instantiate(tilePrefab, transform)
            .With(t => t.Tile.Position = position)
            .With(t => t.Tile.Player = game.Engine.Player)
            .With(t => t.Width = matchConfig.TileWidth)
            .With(t => t.Build(animConfig.TileBuildDuration));
        yield break;
    }


    public IEnumerator Break(Vector2Int position)
    {
        //Destroy(_field[position].gameObject);
        _field[position].Break(animConfig.TileBreakDuration);
        _field.Remove(position);
        yield break;
    }

    public IEnumerator Kill(Vector2Int position)
    {
        var occ = _field[position].Tile.Occupant;
        _field[position].Tile.Occupant = null;
        occ.CardAnimation.Die(); /*
        _field[position].Occupant = null;*/
        yield break;
    }

    public IEnumerator Push(Vector2Int start, Vector2Int finish)
    {
        yield return Move(start, finish, 1);
    }

    public IEnumerator Pull(Vector2Int start, Vector2Int finish)
    {
        yield return Move(start, finish, 1);
    }

    private IEnumerator Move(Vector2Int start, Vector2Int finish, float duration)
    {
        var occupantRect = _field[start].Tile.Occupant.Rect;
        occupantRect
            .DOAnchorPos(
                occupantRect.anchoredPosition +
                matchConfig.TileWidth * (finish - start), duration)
            .OnComplete(() =>
            {
                _field[finish].Tile.Occupant = _field[start].Tile.Occupant;
                _field[finish].Tile.Occupant.Parent = _field[finish];
                _field[start].Tile.Occupant = null;
            })
            .Play();


        yield return new WaitForSeconds(duration);
    }
}