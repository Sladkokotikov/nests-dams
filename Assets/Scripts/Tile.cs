using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour , IPointerClickHandler
{
    //[SerializeField] private RectTransform rect;
    [SerializeField] private RectTransform rect;
    public RectTransform Rect => rect;
    [SerializeField] private RectTransform innerRect;
    [SerializeField] private Image innerRectImage;
    private Vector2Int _position;
    public RealPlayer Player;
    private int _width;

    public int Width
    {
        set
        {
            _width = value;
            rect.sizeDelta = _width * Vector2.one;
            innerRect.sizeDelta = (_width - 20) * Vector2.one;
            Position = Position;
        }
    }

    public Vector2Int Position
    {
        get => _position;
        set
        {
            _position = value;
            rect.anchoredPosition = _width * value;
        }
    }

    private Card _occupant;
    public Card Occupant
    {
        get => _occupant;
        set
        {
            _occupant = value;
            //_occupant.ToField(Position);
            //_occupant.Position = Position;
            if (!_occupant)
                return;
            _occupant.transform.SetParent(transform, true);
            _occupant.Position = Vector2Int.zero;
        }
    }

    public bool Free => Occupant is null;
    public bool Occupied => !Free;
    private bool _possible;

    public bool Possible
    {
        get => _possible;
        set
        {
            _possible = value;
            if (value)
                Highlight();
            else
                StopHighlight();
        }
    }

    public void Highlight()
    {
        innerRectImage.color = Color.yellow;
    }

    public void StopHighlight()
    {
        innerRectImage.color = Color.white;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Player.CanSelectTile||!Possible)
            return;
        
        Player.SelectTile(Position);
    }

    public Vector2 CalculateFuturePosition(Vector2Int destination)
        => _width * destination;
}