using UnityEngine;
using UnityEngine.EventSystems;

public class CollectionCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Card card;
    public Collection collection;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (collection.DeckDisplayed)
            collection.AddToDeck(card.Data);
        else
            Expand();
    }

    private void Expand()
    {
        
    }
}
