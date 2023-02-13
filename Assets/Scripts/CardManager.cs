using System;
using Enums;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private CardData[] data;

    public CardData GetCard(int index) => data[CorrectIndex(index)];

    private int CorrectIndex(int index) =>
        index < 0 || index >= data.Length ? throw new IndexOutOfRangeException() : index;

    public CardData GetCard(ConcreteCards card)
    {
        return card switch
        {
            ConcreteCards.BeaverCub => GetCard(1),
            _ => throw new Exception("No such card. Add to this switch a desired value which can be seen in inspector")
        };
    }
}