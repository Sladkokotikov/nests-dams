using TMPro;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text cardName;

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
}