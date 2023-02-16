using UnityEngine;

namespace Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Animation")]
    public class AnimationConfiguration : ScriptableObject
    {
        public static readonly int Fade = Shader.PropertyToID("_Fade");
        public static readonly int Scale = Shader.PropertyToID("_Scale");
        public static readonly int Glow = Shader.PropertyToID("_Glow");

        [field: SerializeField] public float TileShowDuration { get; private set; }
        [field: SerializeField] public float TileHideDuration { get; private set; }
        [field: SerializeField] public float TileBuildDuration { get; private set; }
        [field: SerializeField] public float TileBreakDuration { get; private set; }
    }
}