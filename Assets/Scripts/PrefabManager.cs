using UnityEngine;

public class PrefabManager:MonoBehaviour
{
    [field: SerializeField] public CardMovement CardPrefab { get; private set; }
    [field: SerializeField] public RectTransform EmptyPrefab { get; private set; }
    [field: SerializeField] public TileAnimation TilePrefab { get; private set; }
}
