using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private CardDataCollection allCards;
    [SerializeField] private TMP_Text money;

    public List<CardData> GetCards(int count)
        => Enumerable.Repeat(0, count).Select(_ => allCards.RandomCard).ToList();

    private void Start()
    {
        money.text = $"{SaveSystem.Balance}";
    }
}