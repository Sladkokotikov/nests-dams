using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public RealPlayer Player;

    [field: SerializeField] public TileAnimation TileAnimation { get; private set; }

    private CardMovement _occupant;

    public CardMovement Occupant
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Player.CanSelectTile || !Possible)
            return;

        Player.SelectTile(Position);
    }

    private bool _possible;

    public bool Possible
    {
        get => _possible;
        set
        {
            _possible = value;
            if (value)
                TileAnimation.Highlight(ServiceLocator.Locator.AnimationManager.TileShowDuration);
            else
                TileAnimation.StopHighlight(ServiceLocator.Locator.AnimationManager.TileHideDuration);
        }
    }
    
    private Vector2Int _position;
    public Vector2Int Position
    {
        get => _position;
        set
        {
            _position = value;
            TileAnimation.Position = value;
        }
    }
}