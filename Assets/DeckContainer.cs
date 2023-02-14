using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text deckName;

    private Deck _deck;
    

    public Deck Deck
    {
        get => _deck;
        set
        {
            _deck = value;
            deckName.text = value.Name;
        }
    }

    public DecksCollection collection;

    public void Show()
    {
        collection.HideDecks();
        collection.Collection.ShowDeck(Deck);
    }
}