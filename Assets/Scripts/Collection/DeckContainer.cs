using TMPro;
using UnityEngine;

public class DeckContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text deckName;
    public int deckIndex;
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

    public Collection collection;

    public void Show()
    {
        collection.ShowDeck(Deck, deckIndex);
    }
}