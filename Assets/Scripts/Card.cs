using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IDraggable
{
    [SerializeField] private RectTransform rect;

    public Vector3 RectPosition
    {
        get => rect.position;
        set => rect.position = value;
    }

    [SerializeField] private RectTransform headArea;
    public RectTransform hand;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardAbility;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image rarityBackground;

    private CardData _data;

    public CardData Data
    {
        get => _data;
        set
        {
            _data = value;
            cardName.text = _data.Name;
            cardAbility.text = _data.AbilityMask;
        }
    }


    public RealPlayer Player;

    public Tile Parent { get; set; }

    public bool InHand => Parent is null;
    public bool OnBoard => !InHand;

    private int _siblingIndex;

    private Vector2Int _position;

    public Vector2Int Position
    {
        get => _position;
        set
        {
            _position = value;
            rect.anchoredPosition = Game.TileWidth * value - headArea.anchoredPosition;
        }
    }

    public RectTransform Rect => rect;

    void Start()
    {
        rarityBackground.color = new Color(Random.value, Random.value, Random.value, 0.7f);
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
        headArea.DOSizeDelta(Vector2.one * 100, 0.2f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Player.CanPlayCard || OnBoard)
            return;
        Drop();
    }

    public void Drop()
    {
        var position = Player.Game.PlacePosition(headArea.position);
        if (position is null)
        {
            cardName.gameObject.SetActive(true);
            cardAbility.gameObject.SetActive(true);
            headArea.DOSizeDelta(new Vector2(300, 500), 0.2f);
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
        rect.position = position - headArea.anchoredPosition;
    }

    public void ToField(Vector2Int position)
    {
        //transform.SetParent(Player.Game.transform);
        rect.anchorMin = rect.anchorMax = Vector2.one / 2;
        headArea.sizeDelta = Vector2.one * 100;
        Position = position;
    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        /*if (InHand)
            return;
        CardInfoVisualizer.Instance.ShowCardInfo(this);#1#
    }*/

    public Vector2 CalculateFuturePosition(Vector2Int finish)
        => Game.TileWidth * finish - headArea.anchoredPosition;
}