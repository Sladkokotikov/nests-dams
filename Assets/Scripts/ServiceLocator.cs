
using UnityEngine;

public class ServiceLocator:MonoBehaviour
{
    public static ServiceLocator Locator;

    private void Awake()
    {
        if (Locator != null)
            Destroy(gameObject);
        else
            Locator = this;
    }

    [field: SerializeField] public PrefabManager PrefabManager { get; private set; }
    [field: SerializeField] public CardManager CardManager { get; private set; }
    [field: SerializeField] public SpriteManager SpriteManager { get; private set; }
    [field: SerializeField] public ConfigurationManager ConfigurationManager { get; private set; }
    [field: SerializeField] public AnimationManager AnimationManager { get; private set; }
}