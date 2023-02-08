using UnityEngine;

public class ServiceLocator:MonoBehaviour
{
    public static ServiceLocator Locator;

    private void Start()
    {
        if (Locator != null)
            Destroy(gameObject);
        else
            Locator = this;
    }

    [field: SerializeField] public CardManager CardManager { get; private set; }
}