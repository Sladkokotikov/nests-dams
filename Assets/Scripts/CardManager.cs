using System;
using System.Collections.Generic;
using UnityEngine;

public class CardManager: MonoBehaviour
{
    [SerializeField] private CardData[] data;

    public CardData GetCard(int index) => data[CorrectIndex(index)];

    private int CorrectIndex(int index) =>
        index < 0 || index >= data.Length ? throw new IndexOutOfRangeException() : index;
}