using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDraggable
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Material dissolve;

    public Vector3 RectPosition
    {
        get => rect.position;
        set => rect.position = value;
    }

    public RectTransform hand;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardAbility;
    [SerializeField] private Image image;
    [SerializeField] private Image outlineImage;
    [SerializeField] private RectTransform imageRect;

    [SerializeField] private Image background;

    //[SerializeField] private RectTransform backgroundRect;
    private Color _color;

    private CardData _data;

    public CardData Data
    {
        get => _data;
        set
        {
            _data = value;
            cardName.text = _data.cardName;
            cardAbility.text = _data.abilityMask;
            image.sprite = _data.image;
            print($"Data set to {_data.cardName}");
        }
    }


    public RealPlayer Player;

    public Tile Parent { get; set; }

    public bool InHand => Parent is null;
    public bool OnBoard => !InHand;

    private int _siblingIndex;

    private Vector2Int _position;
    private static readonly int Fade = Shader.PropertyToID("_Fade");
    private static readonly int Glow = Shader.PropertyToID("_Glow");

    public Vector2Int Position
    {
        get => _position;
        set
        {
            _position = value;
            rect.anchoredPosition = Game.TileWidth * value;
        }
    }

    public RectTransform Rect => rect;

    void Start()
    {
        _color = background.color = new Color(Random.value, Random.value, Random.value, 0.7f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!Player.CanPlayCard || OnBoard)
            return;
        Lift();
    }

    public void Lift()
    {
        _siblingIndex = transform.GetSiblingIndex();
        transform.SetParent(Player.Game.transform);
        cardName.gameObject.SetActive(false);
        cardAbility.gameObject.SetActive(false);
        image.sprite = _data.icon;
        imageRect.DOSizeDelta(Vector2.one * 100, 0.2f);
        background.color = Color.clear;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Player.CanPlayCard || OnBoard)
            return;
        Drop();
    }

    public void Drop()
    {
        var position = Player.Game.PlacePosition(rect.position);
        if (position is null)
        {
            cardName.gameObject.SetActive(true);
            cardAbility.gameObject.SetActive(true);
            image.sprite = _data.image;
            imageRect.DOSizeDelta(200 * Vector2.one, 0.2f);
            background.color = _color;
            transform.SetParent(hand);
            transform.SetSiblingIndex(_siblingIndex);
            return;
        }

        ToField(position.Value);
        Player.PlayedCard = this;
        Player.CardPosition = position.Value;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!Player.CanPlayCard || OnBoard)
            return;
        Drag(eventData.position);
    }

    private void Drag(Vector2 position)
    {
        rect.position = position;
        rect.position = new Vector3(rect.position.x, rect.position.y, 0);
    }

    public void ToField(Vector2Int position)
    {
        //transform.SetParent(Player.Game.transform);
        rect.anchorMin = rect.anchorMax = Vector2.one / 2;
        cardName.gameObject.SetActive(false);
        cardAbility.gameObject.SetActive(false);
        background.enabled = false;
        image.sprite = _data.icon;
        //imageRect.DOSizeDelta(Vector2.one * 100, 0.2f);
        imageRect.sizeDelta = Vector2.one * 100;
        background.color = Color.clear;
        Position = position;
    }

    public void VisualizeBirth()
    {
        image.material = dissolve;
        outlineImage.material = dissolve;
        image.material.SetFloat(Fade, -0.1f);
        dissolve.SetColor(Glow, Color.green);
        DOTween.To(() => dissolve.GetFloat(Fade),
            f => dissolve.SetFloat(Fade, f),
            0.8f,
            3
        ).OnComplete(() => image.material = outlineImage.material = null);
    }

    public void Die()
    {
        image.material = dissolve;
        outlineImage.material = dissolve;
        image.material.SetFloat(Fade, 0.8f);
        dissolve.SetColor(Glow, Color.red);
        DOTween.To(() => dissolve.GetFloat(Fade),
            f => dissolve.SetFloat(Fade, f),
            -0.1f,
            3
        ).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}