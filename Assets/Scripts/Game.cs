using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private RectTransform hand;
    [SerializeField] private RectTransform cardSpawnPoint;
    [SerializeField] private GoalPlace[] goalWaiters;

    private GameEngine _engine;


    private Dictionary<Vector2Int, TileAnimation> _field;


    [SerializeField] private TMP_Text message;

    private void Start()
    {
        Tilt();
        _engine = new GameEngine(this, ServiceLocator.Locator.ConfigurationManager.FieldSize);


        StartCoroutine(_engine.Play());
    }

    private void Tilt() => rect.anchoredPosition +=
        ServiceLocator.Locator.ConfigurationManager.TileWidth / 2f * new Vector2(
            1 - ServiceLocator.Locator.ConfigurationManager.FieldSize.x % 2,
            1 - ServiceLocator.Locator.ConfigurationManager.FieldSize.y % 2);

    private List<TileAnimation> _temporaryTiles;

    public IEnumerator ShowPossibleTiles(IEnumerable<Vector2Int> selectedPoints)
    {
        _temporaryTiles = new List<TileAnimation>();
        foreach (var point in selectedPoints.Shuffled().ToList())
        {
            var tile = _field.ContainsKey(point)
                ? _field[point]
                : Instantiate(ServiceLocator.Locator.PrefabManager.TilePrefab, transform)
                    .With(t => t.Tile.Position = point)
                    .With(t => t.Width = ServiceLocator.Locator.ConfigurationManager.TileWidth)
                    .With(t => t.Tile.Player = _engine.Player)
                    .With(t => t.Build(ServiceLocator.Locator.AnimationManager.TileShowDuration));
            tile.Tile.Possible = true;
            if (!_field.ContainsKey(point))
                _temporaryTiles.Add(tile);
        }

        yield return new WaitForSeconds(ServiceLocator.Locator.AnimationManager.TileShowDuration);
    }

    public void Alert(string msg)
    {
        message.text = msg;
        DOTween.Sequence()
            .Append(message.DOFade(1, 1))
            .Append(message.DOFade(0, 1))
            .Play();
    }

    public void Win(Player player, Goal goal)
    {
        Alert($"{player.Name} победил!");
        foreach (var tile in goal.Positions())
            _field[tile].Highlight(ServiceLocator.Locator.AnimationManager.WinShowDuration);
    }

    public IEnumerator ShowField(Dictionary<Vector2Int, TileInfo> field)
    {
        _field = new Dictionary<Vector2Int, TileAnimation>();
        foreach (var position in field.Keys.Shuffled())
        {
            yield return Build(position);
            yield return new WaitForSeconds(0.08f);
        }
    }

    private bool CanPlaceCard(Vector2Int pos)
        => _field.ContainsKey(pos) && _field[pos].Tile.Free;

    public Vector2Int? PlacePosition(Vector3 cardPosition)
    {
        var realPos = cardPosition - rect.position;
        var pos = Vector2Int.RoundToInt(realPos / ServiceLocator.Locator.ConfigurationManager.TileWidth);
        if (CanPlaceCard(pos))
            return pos;
        return null;
    }

    public void PlaceCard(CardMovement card, Vector2Int position)
    {
        card.ToField(position);
        //card.transform.SetParent(_field[position].transform, true);
        _field[position].Tile.Occupant = card;
        card.Parent = _field[position];
    }

    public IEnumerator ShowCard(CardData data)
    {
        var handPos = hand.position + Vector3.right * 120 * hand.childCount;
        var dummy = Instantiate(ServiceLocator.Locator.PrefabManager.EmptyPrefab)
            .With(r => r.SetParent(hand));


        var card = Instantiate(ServiceLocator.Locator.PrefabManager.CardPrefab, transform)
            .With(c => c.Player = _engine.Player)
            .With(c => c.Card.Data = data)
            .With(c => c.hand = hand)
            .With(c => c.RectPosition = cardSpawnPoint.position);

        const float drawDuration = 1f;
        DOTween.To(
                () => card.RectPosition,
                v => card.RectPosition = v,
                handPos,
                drawDuration)
            .OnComplete(() =>
            {
                Destroy(dummy.gameObject);
                card.transform.SetParent(hand);
            })
            .Play();

        yield return new WaitForSeconds(drawDuration);
    }

    public IEnumerator HidePossibleTiles(IEnumerable<Vector2Int> possibleTiles)
    {
        foreach (var tile in possibleTiles.Where(p => _field.ContainsKey(p)))
            _field[tile].Tile.Possible = false;

        foreach (var tile in _temporaryTiles)
            tile.Break(ServiceLocator.Locator.AnimationManager.TileHideDuration);


        //Destroy(tile.gameObject);
        yield return new WaitForSeconds(ServiceLocator.Locator.AnimationManager.TileHideDuration);
        _temporaryTiles = new List<TileAnimation>();
    }

    public IEnumerator Build(Vector2Int position)
    {
        _field[position] = Instantiate(ServiceLocator.Locator.PrefabManager.TilePrefab, transform)
            .With(t => t.Tile.Position = position)
            .With(t => t.Tile.Player = _engine.Player)
            .With(t => t.Width = ServiceLocator.Locator.ConfigurationManager.TileWidth)
            .With(t => t.Build(ServiceLocator.Locator.AnimationManager.TileBuildDuration));
        yield break;
    }

    public IEnumerator CreateAndPlaceCard(CardData card, Vector2Int position, bool spawn)
    {
        Instantiate(ServiceLocator.Locator.PrefabManager.CardPrefab, transform)
            .With(c => c.Player = _engine.Player)
            .With(c => c.Card.Data = card)
            .With(c => PlaceCard(c, position))
            .With(c => c.CardAnimation.VisualizeBirth(), spawn);
        yield break;
    }

    public IEnumerator Break(Vector2Int position)
    {
        //Destroy(_field[position].gameObject);
        _field[position].Break(ServiceLocator.Locator.AnimationManager.TileBreakDuration);
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
                ServiceLocator.Locator.ConfigurationManager.TileWidth * (finish - start), duration)
            .OnComplete(() =>
            {
                _field[finish].Tile.Occupant = _field[start].Tile.Occupant;
                _field[finish].Tile.Occupant.Parent = _field[finish];
                _field[start].Tile.Occupant = null;
            })
            .Play();


        yield return new WaitForSeconds(duration);
    }

    public void SavePlayerGoals(IEnumerable<Goal> goals)
    {
        var i = 0;
        foreach (var goal in goals)
        {
            goalWaiters[i].Init(goal);
            i++;
        }
    }
}