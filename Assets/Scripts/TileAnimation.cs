using Configuration;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TileAnimation : MonoBehaviour
{
    [SerializeField] private Material dissolve;
    [SerializeField] private RectTransform innerRect;
    [SerializeField] private RectTransform outerRect;
    [SerializeField] private Image innerImage;
    [SerializeField] private Image outerImage;
    [field: SerializeField] public Tile Tile { get; private set; }

    [SerializeField] private TileConfiguration tileConfig;

    public void Break(float duration)
    {
        var newMaterial = Instantiate(dissolve);
        outerImage.enabled = false;
        innerImage.material = outerImage.material = newMaterial;

        newMaterial.Animate(0.8f, -0.1f, Color.red, 80, duration, () => { Destroy(gameObject); });
    }

    public void Build(float duration)
    {
        var newMaterial = Instantiate(dissolve);
        outerImage.enabled = false;
        innerImage.material = newMaterial;
        newMaterial.SetFloat(AnimationConfiguration.Fade, -0.1f);
        newMaterial.SetFloat(AnimationConfiguration.Scale, 80);
        newMaterial.SetColor(AnimationConfiguration.Glow, new Color(1, 0.3f, 0));
        DOTween.To(() => newMaterial.GetFloat(AnimationConfiguration.Fade),
            f => newMaterial.SetFloat(AnimationConfiguration.Fade, f),
            0.8f,
            duration
        ).OnComplete(() =>
        {
            innerImage.material = null;
            outerImage.enabled = true;
        });
    }

    private int _width;


    public int Width
    {
        set
        {
            _width = value;
            outerRect.sizeDelta = _width * Vector2.one;
            innerRect.sizeDelta = _width * Vector2.one;
            Position = Tile.Position;
        }
    }

    public Vector2Int Position
    {
        set => outerRect.anchoredPosition = _width * value;
    }

    public void Highlight()
    {
        innerImage.DOColor(Color.yellow, tileConfig.ShowDuration);
    }

    public void StopHighlight()
    {
        innerImage.DOColor(Color.white, tileConfig.HideDuration);
    }

    private void Start()
    {
        innerImage.sprite = tileConfig.Floor.Choose();
    }
}