using System.Collections;
using TMPro;
using UnityEngine;

public class CardInfoVisualizer : MonoBehaviour
{
    public static CardInfoVisualizer Instance;
    private static bool _instanceCreated;
    [SerializeField] private RectTransform rect;
    [SerializeField] private TMP_Text shownCardName;
    [SerializeField] private TMP_Text shownCardAbility;

    private Coroutine _destroyer;
    private bool _waitingDestroy;

    private IEnumerator Destroy()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        gameObject.SetActive(false);
    }

    private void Start()
    {
        if (_instanceCreated)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _instanceCreated = true;
    }

    public void ShowCardInfo(Card card)
    {
        if (_waitingDestroy)
            StopCoroutine(_destroyer);
        gameObject.SetActive(true);
        print($"{card.Data.Name} {card.Data.AbilityMask}");
        shownCardName.text = card.Data.Name;
        rect.position = card.RectPosition + new Vector3(-200, 200);
        shownCardAbility.text = card.Data.AbilityMask;
        _destroyer = StartCoroutine(Destroy());
        _waitingDestroy = true;
    }
}