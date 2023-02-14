using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private CardData[] data;

    public CardData GetCard(int index) => data[CorrectIndex(index)];

    private int CorrectIndex(int index) =>
        index < 0 || index >= data.Length ? throw new IndexOutOfRangeException() : index;

    [SerializeField] private CardData beaverCub;
    [SerializeField] private CardData magpieFledgling;

    [field: SerializeField] public CardData Obstacle { get; private set; }

    public CardData GetCard(ConcreteCard card)
    {
        switch (card)
        {
            case ConcreteCard.BeaverCub:
                return beaverCub;
            case ConcreteCard.MagpieFledgling:
                return magpieFledgling;
        }

        throw new Exception("No such card");
    }

    [SerializeField] private int[] startDeck;
    public List<CardData> StartDeck => startDeck.Select(GetCard).ToList();

    [SerializeField] private int[] startBotDeck;
    public List<CardData> StartBotDeck => startBotDeck.Select(GetCard).ToList();
}