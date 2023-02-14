using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardsCollection : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private CardMovement itemPrefab;
    [SerializeField] private CardManager cardManager;

    public IEnumerator Init()
    {
        var collection = SaveSystem.GetCollection().ToArray();

        var group = panel.GetComponent<GridLayoutGroup>();
        panel.sizeDelta =
            new Vector2(1300,
                group.spacing.y + (collection.Length / group.constraintCount +
                                   (collection.Length % group.constraintCount > 0 ? 1 : 0)) *
                (group.cellSize.y + group.spacing.y));

        foreach (var cardId in collection)
        {
            var card = Instantiate(itemPrefab, panel);
            card.Card.Data = cardManager.GetCard(cardId);
            card.enabled = false;
            yield return null;
        }
    }
}