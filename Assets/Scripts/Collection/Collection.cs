using System.Collections;
using UnityEngine;

public class Collection : MonoBehaviour
{
    [field: SerializeField] public CardsCollection CardsCollection { get; private set; }
    [SerializeField] private DecksCollection decksCollection;
    [SerializeField] private DeckDisplay deckDisplay;

    public bool DeckDisplayed { get; private set; }

    private void Start()
    {
        PlayerPrefs.DeleteAll();

        StartCoroutine(CardsCollection.Init());
        ShowAllDecks();
    }

    public void ShowDeck(Deck deck, int index)
    {
        decksCollection.gameObject.SetActive(false);
        
        deckDisplay.gameObject.SetActive(true);
        StartCoroutine(deckDisplay.Init(deck, index));
        DeckDisplayed = true;
    }

    public void AddToDeck(CardData cardData)
    {
        deckDisplay.Add(cardData);
    }

    public void ShowAllDecks()
    {
        /*StartCoroutine(deckDisplay.Hide());
        StartCoroutine(decksCollection.Init());*/
        deckDisplay.gameObject.SetActive(false);
        decksCollection.gameObject.SetActive(true);
        StartCoroutine(decksCollection.Init());
    }
}