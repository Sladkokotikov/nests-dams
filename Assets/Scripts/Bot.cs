using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Player
{
    public Bot(GameEngine engine, string name, List<Goal> goals, List<CardData> deck) : base(engine, name, goals, deck)
    {
    }

    public override IEnumerator MakeTurn()
    {
        if (Hand.Count == 0)
            yield break;
        var card = Hand[0];

        yield return Engine.PlayBotCard(card);
        Hand.RemoveAt(0);
    }

    public override bool CanPlayCard
    {
        get => throw new Exception("Bot tries to get CanPlayCard property, it is is prohibited");
        protected set => throw new Exception("Bot tries to set CanPlayCard property, it is is prohibited");
    }

    public override void PlayCard(CardMovement card, Vector2Int position)
        => throw new Exception("Bot tries to PlayCard, it is prohibited");

    public override IEnumerator DrawCard()
    {
        if (Deck.Count == 0)
            yield break;
        var card = Deck[0];
        Hand.Add(card);
        Deck.RemoveAt(0);
    }
}