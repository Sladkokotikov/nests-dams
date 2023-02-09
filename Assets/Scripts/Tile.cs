using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Image image;
    private Vector2Int _position;
    public RealPlayer Player;
    private int _width;

    public int Width
    {
        set
        {
            _width = value;
            rect.sizeDelta = _width * Vector2.one;
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
        image.color = Color.yellow;
    }

    private void StopHighlight()
    {
        image.color = Color.white;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Player.CanSelectTile||!Possible)
            return;
        
        Player.SelectTile(Position);
    }
}