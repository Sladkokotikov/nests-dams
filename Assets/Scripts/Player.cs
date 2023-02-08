using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Player
{
    public string Name { get; }
    protected List<Goal> Goals;
    public IEnumerable<Goal> GetGoals => Goals;
    public List<CardData> Deck, Hand, Graveyard, Discard;
    public GameEngine Engine { get; set; }
    public Game Game => Engine.Game;

    public abstract IEnumerator MakeTurn();

    public Goal CompletedGoal(Dictionary<Vector2Int, TileInfo> field)
        => Goals.FirstOrDefault(goal => goal.IsSatisfied(field));

    protected Player(GameEngine engine, string name, List<Goal> goals, List<CardData> deck)
    {
        Hand = new List<CardData>();
        Deck = deck;
        Graveyard = new List<CardData>();
        Discard = new List<CardData>();

        Engine = engine;
        Name = name;
        Goals = goals;
    }

    public abstract IEnumerator DrawCard();
}