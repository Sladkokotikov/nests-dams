using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CardDataCollection")]
public class CardDataCollection : ScriptableObject
{
    [SerializeField] private CardData[] cards;

    public int Count => cards.Length;
    
    private static Dictionary<int, CardData> _cache;
    
    public CardData GetCardByID(int id)
        => (_cache ??= cards.ToDictionary(c => c.Id, c => c))
            .ContainsKey(id)
                ? _cache[id]
                : throw new ArgumentException("Card with this ID is not presented");

    [SerializeField] private CardData beaverCub;
    [SerializeField] private CardData magpieFledgling;

    public CardData GetConcreteCard(ConcreteCard card)
    {
        switch (card)
        {
            case ConcreteCard.BeaverCub:
                return beaverCub;
            case ConcreteCard.MagpieFledgling:
                return magpieFledgling;
        }

        throw new ArgumentException($"Concrete card {card} can't be found");
    }

    public CardData RandomCard
        => (_cache ??= cards.ToDictionary(c => c.Id, c => c)).Choose().Value;

    [field: SerializeField] public CardData Obstacle { get; private set; }
}