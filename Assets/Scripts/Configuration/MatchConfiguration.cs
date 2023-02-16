using UnityEngine;

namespace Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Match")]
    public class MatchConfiguration : ScriptableObject
    {
        [field: SerializeField] public Vector2Int FieldSize { get; private set; }
        [field: SerializeField] public int TileWidth { get; private set; }
    }
}