using UnityEngine;

namespace Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Tile")]
    public class TileConfiguration : ScriptableObject
    {
        [field: SerializeField] public Sprite[] Floor { get; private set; }
        [field: SerializeField] public float ShowDuration { get; private set; }
        [field: SerializeField] public float HideDuration { get; private set; }
    }
}