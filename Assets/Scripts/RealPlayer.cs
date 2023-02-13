using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealPlayer : Player
{
    public bool CanPlayCard { get; private set; }

    private bool IsCardPlayed { get; set; }
    public Card PlayedCard { get; set; }
    public Vector2Int CardPosition { get; set; }

    public bool CanSelectTile { get; private set; }
    private bool IsTileSelected { get; set; }
    public Vector2Int TilePosition { get; private set; }

    public RealPlayer(GameEngine engine, string name, List<Goal> goals, List<CardData> deck) : base(engine, name, goals,
        deck)
    {
    }

    public override IEnumerator MakeTurn()
    {
        yield return GetCard();
        if (IsCardPlayed)
            yield return Engine.PlaceCard(PlayedCard, CardPosition);
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
        TilePosition = position;
        IsTileSelected = true;
    }

    public void Reset()
    {
        IsTileSelected = false;
        IsCardPlayed = false;
    }

    public void PlayCard(Card card, Vector2Int position)
    {
        PlayedCard = card;
        CardPosition = position;
        IsCardPlayed = true;
    }
}