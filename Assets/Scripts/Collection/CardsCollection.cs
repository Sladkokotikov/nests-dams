using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsCollection : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private CollectionCardContainer itemPrefab;
    [SerializeField] private Collection collection;

    [SerializeField] private CardDataCollection allCards;

    public Dictionary<int, int> Collection { get; private set; }

    public IEnumerator Init()
    {
        Collection = SaveSystem.Collection;

        foreach (var pair in Collection)
        {
            var card = Instantiate(itemPrefab, panel);
            card.Data = allCards.GetCardByID(pair.Key);
            card.Amount = pair.Value;
            card.Collection = collection;
            card.enabled = false;
            yield return null;
        }
    }
}