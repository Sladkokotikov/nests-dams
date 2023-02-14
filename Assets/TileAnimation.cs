using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TileAnimation : MonoBehaviour
{
    [SerializeField] private Material dissolve;
    [field: SerializeField] public RectTransform InnerRect { get; private set; }
    [field: SerializeField] public RectTransform OuterRect { get; private set; }
    [field: SerializeField] public Image InnerImage { get; private set; }
    [field: SerializeField] public Image OuterImage { get; private set; }
    [field: SerializeField] public Tile Tile { get; private set; }

    public void Break(float duration)
    {
        var newMaterial = Instantiate(dissolve);
        OuterImage.enabled = false;
        InnerImage.material = OuterImage.material = newMaterial;

        newMaterial.SetFloat(ServiceLocator.Locator.ConfigurationManager.Fade, 0.8f);
        newMaterial.SetFloat(ServiceLocator.Locator.ConfigurationManager.Scale, 80);
        newMaterial.SetColor(ServiceLocator.Locator.ConfigurationManager.Glow, Color.red);
        DOTween.To(() => newMaterial.GetFloat(ServiceLocator.Locator.ConfigurationManager.Fade),
            f => newMaterial.SetFloat(ServiceLocator.Locator.ConfigurationManager.Fade, f),
            -0.1f,
            duration
        ).OnComplete(() => { Destroy(gameObject); });
    }

    public void Build(float duration)
    {
        var newMaterial = Instantiate(dissolve);
        OuterImage.enabled = false;
        InnerImage.material = newMaterial;
        newMaterial.SetFloat(ServiceLocator.Locator.ConfigurationManager.Fade, -0.1f);
        newMaterial.SetFloat(ServiceLocator.Locator.ConfigurationManager.Scale, 80);
        newMaterial.SetColor(ServiceLocator.Locator.ConfigurationManager.Glow, new Color(1, 0.3f, 0));
        DOTween.To(() => newMaterial.GetFloat(ServiceLocator.Locator.ConfigurationManager.Fade),
            f => newMaterial.SetFloat(ServiceLocator.Locator.ConfigurationManager.Fade, f),
            0.8f,
            duration
        ).OnComplete(() =>
        {
            InnerImage.material = null;
            OuterImage.enabled = true;
        });
    }
    private int _width;
    public int Width
    {
        set
        {
            _width = value;
            OuterRect.sizeDelta = (_width) * Vector2.one;
            InnerRect.sizeDelta = (_width - ServiceLocator.Locator.ConfigurationManager.TileOffsetRange) * Vector2.one;
            Position = Tile.Position;
        }
    }
    public Vector2Int Position
    {
        set => OuterRect.anchoredPosition = _width * value;
    }
    
    public void Highlight(float duration)
    {
        InnerImage.DOColor(Color.yellow, duration);
    }

    public void StopHighlight(float duration)
    {
        InnerImage.DOColor(Color.white, duration);
    }
    
    private void Start()
    {
        InnerImage.sprite = ServiceLocator.Locator.SpriteManager.Floor.Choose();
    }
    
}