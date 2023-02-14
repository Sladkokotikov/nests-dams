using System.Collections;
using UnityEngine;

public class Collection : MonoBehaviour
{
    [SerializeField] private CardsCollection cardsCollection;
    [SerializeField] private DecksCollection decksCollection;
    [SerializeField] private DeckDisplay deckDisplay;

    private void Start()
    {
        StartCoroutine(cardsCollection.Init());
        StartCoroutine(decksCollection.Init());
    }


    public void ShowDeck(Deck deck)
    {
        StartCoroutine(deckDisplay.Init(deck));
    }
}