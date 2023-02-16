using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck
{
    public readonly string Name;
    public readonly Dictionary<int, int> Cards;

    private Deck(string name, Dictionary<int, int> cards)
    {
        Name = name;
        Cards = cards;
    }

    public override string ToString()
        => $"{Name}-{string.Join(",", Cards.Select(p => $"{p.Key}:{p.Value}"))}";

    public static Deck FromString(string source)
    {
        Debug.Log(source);
        var split = source.Split('-');
        var name = split[0];
        var cards = split[1] == string.Empty
            ? new Dictionary<int, int>()
            : split[1].Split(',')
                .ToDictionary(s => int.Parse(s.Split(':')[0]), s => int.Parse(s.Split(':')[1]));
        return new Deck(name, cards);
    }

    public static Deck Create(string name, Dictionary<int, int> cards)
        => new Deck(name, cards);
}