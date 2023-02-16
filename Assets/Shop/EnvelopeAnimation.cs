using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnvelopeAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Image image;
    [SerializeField] private Vector2 smoothMovementDurationRange;
    [SerializeField] private float radius = 20;
    [SerializeField] private float scale;
    [SerializeField] private float crackDuration;
    [SerializeField] private float rangeModifier;
    [SerializeField] private float radiusModifier;

    [SerializeField] private Shop shop;

    [SerializeField] private ShopCard shopCard;

    [SerializeField] private Material dissolve;


    private void Start()
    {
        image.material = Instantiate(dissolve);
        StartCoroutine(SmoothRandomMovement());
    }

    private IEnumerator SmoothRandomMovement()
    {
        var destination = radius * Random.insideUnitCircle;
        var duration = Random.Range(smoothMovementDurationRange.x, smoothMovementDurationRange.y);
        rect.DOAnchorPos(destination, duration);
        yield return new WaitForSeconds(duration);
        yield return SmoothRandomMovement();
    }

    public void Break()
    {
        smoothMovementDurationRange *= rangeModifier;
        radius *= radiusModifier;
        image.material.Animate(0.8f, -0.1f, Color.yellow, scale, crackDuration, ShowCards);
    }

    private void ShowCards()
    {
        var cards = shop.GetCards(3);
        shopCard.gameObject.SetActive(true);
        shopCard.LoadCards(cards);
        SaveSystem.AddToCollection(cards);
    }
}