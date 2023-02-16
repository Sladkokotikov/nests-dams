using UnityEngine;

namespace Configuration
{
    [CreateAssetMenu(menuName = "Configuration/Bot")]
    public class BotConfiguration : ScriptableObject
    {
        [SerializeField] private Vector2 thinkRange;
        public float ThinkRange => Random.Range(thinkRange.x, thinkRange.y);
    }
}