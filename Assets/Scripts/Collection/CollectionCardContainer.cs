using TMPro;
using UnityEngine;

public class CollectionCardContainer : MonoBehaviour
{
    [SerializeField] private Card card;
    [SerializeField] private CollectionCard collectionCard;

    private CardData _data;

    public CardData Data
    {
        set => _data = card.Data = value;
    }

    [SerializeField] private TMP_Text amount;
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

    public Collection Collection
    {
        set => collectionCard.collection = value;
    }
}