using UnityEngine;

public class Card : MonoBehaviour
{
    [field: SerializeField] public CardAnimation CardAnimation { get; private set; }
    [field:  SerializeField] public CardMovement CardMovement{ get; private set; }

    private CardData _data;

    public CardData Data
    {
        get => _data;
        set
        {
            _data = value;
            CardAnimation.Set();
        }
    }
}