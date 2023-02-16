using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecksCollection : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private DeckContainer itemPrefab;

    [SerializeField] private float fadeDuration;

    [SerializeField] private Collection collection;

    public IEnumerator Init()
    {
        panel.Clear(1);

        // yield return canvasGroup.FastAppear();
        var decks = SaveSystem.GetDecks().ToArray();

        var i = 0;
        foreach (var deck in decks)
        {
            var item = Instantiate(itemPrefab, panel);
            item.Deck = deck;
            item.collection = collection;
            item.deckIndex = i++;
            yield return null;
        }
    }

    public void AddDeck()
    {
        var deck = Deck.Create("New Deck", new Dictionary<int, int>());
        SaveSystem.AddDeck(deck);
        collection.ShowDeck(deck, 0);
    }
}