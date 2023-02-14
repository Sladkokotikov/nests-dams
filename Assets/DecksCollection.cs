using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DecksCollection : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private DeckContainer itemPrefab;

    [SerializeField] private float fadeDuration;

    [field: SerializeField] public Collection Collection { get; private set; }

    public IEnumerator Init()
    {
        var decks = SaveSystem.GetDecks().ToArray();

        var group = panel.GetComponent<GridLayoutGroup>();
        panel.sizeDelta =
            new Vector2(520,
                group.spacing.y + (decks.Length / group.constraintCount +
                                   (decks.Length % group.constraintCount > 0 ? 1 : 0)) *
                (group.cellSize.y + group.spacing.y));

        foreach (var deck in decks)
        {
            var item = Instantiate(itemPrefab, panel);
            item.Deck = deck;
            item.collection = this;
            yield return null;
        }
    }

    public void HideDecks()
    {
        canvasGroup.DOFade(0, fadeDuration).OnComplete(()=>canvasGroup.gameObject.SetActive(false));
    }
}