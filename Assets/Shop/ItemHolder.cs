using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
   

    [SerializeField] private GameObject smallView;
    [SerializeField] private GameObject fullView;

    [SerializeField] private TMP_Text smallName;
    [SerializeField] private TMP_Text fullName;
    
    [SerializeField] private TMP_Text smallPrice;
    [SerializeField] private TMP_Text fullPrice;
    
    [SerializeField] private TMP_Text description;

    [SerializeField] private Image smallImage;
    [SerializeField] private Image fullImage;
    
    [SerializeField] private float errDuration;

    [SerializeField] private Item item;


    private void OnValidate()
    {
        
        smallPrice.text = fullPrice.text = $"{item.Price}";
        description.text = item.Description;
        smallName.text = fullName.text = item.Name;
        smallImage.sprite = fullImage.sprite = item.Sprite;
    }

    public void Err()
    {
        /*DOTween.Sequence()
            .Append(price.DOColor(Color.red, errDuration))
            .Append(price.DOColor(Color.white, errDuration));*/
    }

    public void Expand()
    {
        smallView.SetActive(false);
        fullView.SetActive(true);
    }
    
    
}