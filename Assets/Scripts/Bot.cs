using System.Collections;
using System.Collections.Generic;

public class Bot : Player
{
    public Bot(GameEngine engine, string name, List<Goal> goals, List<CardData> deck):base(engine, name, goals, deck)
    {
    }

    public override IEnumerator MakeTurn()
    {
        if(Hand.Count == 0)
            yield break;
        var card = Hand[0];
        
        yield return Engine.PlayBotCard(card);
        Hand.RemoveAt(0);
    }

    public override IEnumerator DrawCard()
    {
        if (Deck.Count == 0)
            yield break;
        var card = Deck[0];
        Hand.Add(card);
        Deck.RemoveAt(0);
    }
}