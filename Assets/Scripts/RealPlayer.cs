using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealPlayer : Player
{
    public bool CanPlayCard { get; private set; }

    public bool IsCardPlayed => !(CardPosition is null);
    public Card PlayedCard { get; set; }
    public Vector2Int? CardPosition { get; set; }

    public bool CanSelectTile { get; private set; }
    private bool IsTileSelected => !(TilePosition is null);
    public Vector2Int? TilePosition { get; private set; }

    public RealPlayer(GameEngine engine, string name, List<Goal> goals, List<CardData> deck) : base(engine, name, goals,
        deck)
    {
    }

    public override IEnumerator MakeTurn()
    {
        yield return GetCard();
        if (CardPosition != null)
            yield return Engine.PlaceCard(PlayedCard, CardPosition.Value);
    }

    public override IEnumerator DrawCard()
    {
        if (Deck.Count == 0)
            yield break;
        var card = Deck[0];
        Hand.Add(card);
        Deck.RemoveAt(0);
        yield return Game.ShowCard(card);
    }

    private IEnumerator GetCard()
    {
        CanPlayCard = true;
        yield return new WaitUntil(() => IsCardPlayed);
        CanPlayCard = false;
    }

    public IEnumerator Confirm()
    {
        CanSelectTile = true;
        yield return new WaitUntil(() => IsTileSelected);
        CanSelectTile = false;
    }

    public void SelectTile(Vector2Int position)
    {
        Debug.Log($"Selected tile: {position}");
        TilePosition = position;
    }

    public void Reset()
    {
        TilePosition = null;
        CardPosition = null;
    }
}