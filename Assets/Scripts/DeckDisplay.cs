using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DeckDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private CardContainer itemPrefab;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration;


    public IEnumerator Init(Deck deck)
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        var collection = deck.Cards;

        var group = panel.GetComponent<GridLayoutGroup>();
        panel.sizeDelta =
            new Vector2(520,
                group.spacing.y + (collection.Length / group.constraintCount +
                                   (collection.Length % group.constraintCount > 0 ? 1 : 0)) *
                (group.cellSize.y + group.spacing.y));

        foreach (var cardId in collection)
        {
            var card = Instantiate(itemPrefab, panel);
            card.Data = cardManager.GetCard(cardId);
            card.enabled = false;
            yield return null;
        }
    }
}