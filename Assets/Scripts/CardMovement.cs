using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDraggable
{
    [field: SerializeField] public Card Card { get; private set; }
    [field: SerializeField] public CardAnimation CardAnimation { get; private set; }
    public RectTransform hand;
    [SerializeField] private RectTransform rect;
    public Player Player;
    public Field field;

    public Vector3 RectPosition
    {
        get => rect.position;
        set => rect.position = value;
    }

    private Vector2Int _position;

    public TileAnimation Parent { get; set; }

    public bool InHand => Parent is null;
    public bool OnBoard => !InHand;

    public int TileWidth { get; set; }

    public Vector2Int Position
    {
        get => _position;
        set
        {
            _position = value;
            rect.anchoredPosition = TileWidth * value;
        }
    }

    public RectTransform Rect => rect;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!Player.CanPlayCard || OnBoard)
            return;
        Lift();
    }

    private int _siblingIndex;

    public void Lift()
    {
        _siblingIndex = transform.GetSiblingIndex();
        transform.SetParent(field.AboveAll);
        CardAnimation.Shrink();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Player.CanPlayCard || OnBoard)
            return;
        Drop();
    }

    private void Drop()
    {
        var position = field.PlacePosition(rect.position);
        if (position is null)
        {
            CardAnimation.Expand();
            transform.SetParent(hand);
            transform.SetSiblingIndex(_siblingIndex);
            return;
        }

        ToField(position.Value);
        Player.PlayCard(this, position.Value);
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
        CardAnimation.Shrink();
        Position = position;
    }
}