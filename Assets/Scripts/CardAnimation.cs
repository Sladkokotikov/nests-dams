using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardAnimation : MonoBehaviour
{
    [SerializeField] private Card card;

    [SerializeField] private Material dissolve;
    [field: SerializeField] public TMP_Text CardName { get; private set; }
    [field: SerializeField] public TMP_Text CardAbility { get; private set; }
    [field: SerializeField] public Image Image { get; private set; }
    [field: SerializeField] public Image OutlineImage { get; private set; }
    [field: SerializeField] public RectTransform ImageRect { get; private set; }
    [field: SerializeField] public Image Background { get; private set; }

    public void VisualizeBirth()
    {
        var newMaterial = Instantiate(dissolve);
        Image.material = OutlineImage.material = newMaterial;
        newMaterial.Animate(-0.1f, 0.8f, Color.green, 20, 3, () => Image.material = OutlineImage.material = null);
    }

    public void Die()
    {
        var newMaterial = Instantiate(dissolve);
        Image.material = OutlineImage.material = newMaterial;
        newMaterial.Animate(0.8f, -0.1f, Color.red, 20, 3, () => { Destroy(gameObject); });
    }

    public void Shrink()
    {
        CardName.gameObject.SetActive(false);
        CardAbility.gameObject.SetActive(false);
        Background.enabled = false;
        Image.sprite = card.Data.Icon;
        ImageRect.sizeDelta = Vector2.one * 100;
        Background.color = Color.clear;
    }

    public void Set()
    {
        CardName.text = card.Data.CardName;
        CardAbility.text = card.Data.AbilityMask;
        Image.sprite = card.Data.Image;
        print($"Data set to {card.Data.CardName}");
    }

    
    public void Expand()
    {
        CardName.gameObject.SetActive(true);
        CardAbility.gameObject.SetActive(true);
        Image.sprite = card.Data.Image;
        ImageRect.DOSizeDelta(200 * Vector2.one, 0.2f);
        Background.enabled = true;
        Background.color = Color.black;
    }
}