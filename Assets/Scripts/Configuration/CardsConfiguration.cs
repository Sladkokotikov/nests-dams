using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Configuration/Cards")]
public class CardsConfiguration : ScriptableObject
{
    [field: SerializeField] public int StartCardsCount { get; private set; }
    [field: SerializeField] public int CardsInDeck { get; private set; }
    [field: SerializeField] public CardDataCollection AllCards { get; private set; }

    public List<CardData> StartDeck
        => SaveSystem
            .SelectedDeck
            .Cards
            .SelectMany(pair => Enumerable.Repeat(pair.Key, pair.Value))
            .Select(id => AllCards.GetCardByID(id))
            .ToList();

    public List<CardData> BotDeck
        => Enumerable.Range(0, CardsInDeck).Select(_ => Random.Range(0, AllCards.Count))
            .Select(id => AllCards.GetCardByID(id)).ToList();
}