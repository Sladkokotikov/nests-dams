using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour
{
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardAbility;
    [SerializeField] private Image cardImage;
    [SerializeField] private RectTransform rect;
    [SerializeField] private float showDuration;
    [SerializeField] private Vector2 endSize;
    private int _dataIndex;

    private CardData CurrentData
    {
        set
        {
            cardName.text = value.CardName;
            cardAbility.text = value.AbilityMask;
            cardImage.sprite = value.Image;
        }
    }

    private List<CardData> _cards;

    public void LoadCards(List<CardData> cards)
    {
        _cards = cards;

        ShowNextCard();
        rect.DOSizeDelta(endSize, showDuration);
    }

    public void ShowNextCard()
    {
        if (_dataIndex == _cards.Count)
            Hide();
        else
            CurrentData = _cards[_dataIndex++];
    }

    private void Hide()
    {
        Destroy(gameObject);
    }
}