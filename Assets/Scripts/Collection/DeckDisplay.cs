using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DeckDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private DeckCardContainer itemPrefab;
    [SerializeField] private Collection collection;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration;
    [SerializeField] private CardDataCollection allCards;

    private Dictionary<int, DeckCardContainer> _containers;

    [SerializeField] private TMP_InputField deckName;

    private int _deckIndex;

    public IEnumerator Init(Deck deck, int index)
    {
        panel.Clear(2);
        deckName.text = deck.Name;
        _deckIndex = index;
        _containers = new Dictionary<int, DeckCardContainer>();
        // yield return canvasGroup.FastAppear();
        var cards = deck.Cards;
        foreach (var pair in cards)
            yield return AddCard(pair.Key, pair.Value);
    }

    private IEnumerator AddCard(int id, int amount)
    {
        var card = Instantiate(itemPrefab, panel);
        card.Data = allCards.GetCardByID(id);
        card.deckDisplay = this;
        card.Amount = amount;
        _containers[id] = card;
        yield return null;
    }

    public void Add(CardData cardData)
    {
        if (_containers.ContainsKey(cardData.Id))
        {
            if (_containers[cardData.Id].Amount < collection.CardsCollection.Collection[cardData.Id])
                _containers[cardData.Id].Amount++;
        }
        else
        {
            StartCoroutine(AddCard(cardData.Id, 1));
        }
    }

    public void Remove(CardData cardData)
    {
        _containers[cardData.Id].Amount--;
        if (_containers[cardData.Id].Amount != 0)
            return;
        Destroy(_containers[cardData.Id].gameObject);
        _containers.Remove(cardData.Id);
    }

    public void Save()
    {
        SaveSystem.UpdateDeck(_deckIndex,
            Deck.Create(deckName.text, _containers.ToDictionary(p => p.Key, p => p.Value.Amount)));
        collection.ShowAllDecks();
    }

    public void Delete()
    {
        SaveSystem.RemoveDeckAt(_deckIndex);
        collection.ShowAllDecks();
    }

    public IEnumerator Hide()
    {
        yield return canvasGroup.FastFade();
    }
}