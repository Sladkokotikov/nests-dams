using TMPro;
using UnityEngine;

public class DeckCardContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text amount;

    public DeckDisplay deckDisplay;

    private CardData _data;

    public CardData Data
    {
        get => _data;
        set
        {
            _data = value;
            cardName.text = value.CardName;
        }
    }

    private int _amount;

    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            amount.text = $"x{value}";
        }
    }

    public void Remove()
    {
        deckDisplay.Remove(_data);
    }
}