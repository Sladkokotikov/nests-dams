using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform innerRect;
    [SerializeField] private RectTransform outerRect;
    [SerializeField] private Image innerImage;
    [SerializeField] private Image outerImage;
    private Vector2Int _position;
    public RealPlayer Player;
    private int _width;
    [SerializeField] private Material dissolve;

    public int Width
    {
        set
        {
            _width = value;
            outerRect.sizeDelta = (_width) * Vector2.one;
            innerRect.sizeDelta = (_width - ServiceLocator.Locator.ConfigurationManager.TileOffsetRange) * Vector2.one;
            Position = Position;
        }
    }

    public Vector2Int Position
    {
        get => _position;
        set
        {
            _position = value;
            outerRect.anchoredPosition = _width * value;
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
        innerImage.color = Color.yellow;
    }

    private void StopHighlight()
    {
        innerImage.color = Color.white;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Player.CanSelectTile || !Possible)
            return;

        Player.SelectTile(Position);
    }

    private void Start()
    {
        innerImage.sprite = ServiceLocator.Locator.SpriteManager.Floor.Choose();
    }

    public void Break()
    {
        var newMaterial = Instantiate(dissolve);
        outerImage.enabled = false;
        innerImage.material = outerImage.material = newMaterial;

        newMaterial.SetFloat(ServiceLocator.Locator.ConfigurationManager.Fade, 0.8f);
        newMaterial.SetFloat(ServiceLocator.Locator.ConfigurationManager.Scale, 80);
        newMaterial.SetColor(ServiceLocator.Locator.ConfigurationManager.Glow, Color.red);
        DOTween.To(() => newMaterial.GetFloat(ServiceLocator.Locator.ConfigurationManager.Fade),
            f => newMaterial.SetFloat(ServiceLocator.Locator.ConfigurationManager.Fade, f),
            -0.1f,
            3
        ).OnComplete(() => { Destroy(gameObject); });
    }

    public void Build()
    {
        var newMaterial = Instantiate(dissolve);
        outerImage.enabled = false;
        innerImage.material = newMaterial;
        newMaterial.SetFloat(ServiceLocator.Locator.ConfigurationManager.Fade, -0.1f);
        newMaterial.SetFloat(ServiceLocator.Locator.ConfigurationManager.Scale, 80);
        newMaterial.SetColor(ServiceLocator.Locator.ConfigurationManager.Glow, new Color(1, 0.3f, 0));
        DOTween.To(() => newMaterial.GetFloat(ServiceLocator.Locator.ConfigurationManager.Fade),
            f => newMaterial.SetFloat(ServiceLocator.Locator.ConfigurationManager.Fade, f),
            0.8f,
            3
        ).OnComplete(() =>
        {
            innerImage.material = null;
            outerImage.enabled = true;
        });
    }
}