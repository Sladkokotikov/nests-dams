using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SaveSystem
{
    public static Dictionary<int, int> Collection
    {
        get => !PlayerPrefs.HasKey(CollectionKey)
            ? Collection = DefaultCollection
            : PlayerPrefs.GetString(CollectionKey).Split(',')
                .ToDictionary(s => int.Parse(s.Split(':')[0]),
                    s => int.Parse(s.Split(':')[1]));
        private set => PlayerPrefs.SetString(CollectionKey, string.Join(",", value.Select(p => $"{p.Key}:{p.Value}")));
    }

    private static Dictionary<int, int> DefaultCollection
        => new Dictionary<int, int> {[0] = 5, [1] = 5, [2] = 5};


    public static List<Deck> GetDecks()
    {
        if (!PlayerPrefs.HasKey(DecksKey))
            return new List<Deck>();
        if (!PlayerPrefs.HasKey(NoDecksKey))
            return new List<Deck>();
        return PlayerPrefs.GetInt(NoDecksKey) == 1
            ? new List<Deck>()
            : PlayerPrefs.GetString(DecksKey).Split(';').Select(Deck.FromString).ToList();
    }


    public static void AddDeck(Deck deck)
    {
        var decks = GetDecks();
        decks.Insert(0, deck);
        SaveDecks(decks);
    }

    public static void SaveDecks(List<Deck> decks)
    {
        var info = string.Join(";", decks.Select(d => d.ToString()));
        PlayerPrefs.SetInt(NoDecksKey, decks.Count == 0 ? 1 : 0);
        PlayerPrefs.SetString(DecksKey, info);
    }

    public static void UpdateDeck(int index, Deck deck)
    {
        var decks = GetDecks();
        decks[index] = deck;
        SaveDecks(decks);
    }

    public static void RemoveDeckAt(int index)
    {
        var decks = GetDecks().ToList();
        decks.RemoveAt(index);

        SaveDecks(decks);
    }

    public static Deck SelectedDeck { get; private set; }


    public static int Balance
    {
        get => PlayerPrefs.HasKey(MoneyKey) ? PlayerPrefs.GetInt(MoneyKey) : Balance = DefaultBalance;
        set => PlayerPrefs.SetInt(MoneyKey, value);
    }

    public static int DefaultBalance => 100;

    private const string DecksKey = "Decks";
    private const string CollectionKey = "Collection";
    private const string NoDecksKey = "NoDecks";
    private const string MoneyKey = "Money";

    public static void AddToCollection(List<CardData> cards)
    {
        var collection = Collection;
        foreach (var card in cards)
        {
            if (!collection.ContainsKey(card.Id))
                collection[card.Id] = 0;
            collection[card.Id]++;
        }

        Collection = collection;
    }
}