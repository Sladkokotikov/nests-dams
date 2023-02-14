using UnityEngine;


public class AnimationManager : MonoBehaviour
{
    [field: SerializeField] public float TileBuildDuration { get; private set; }
    [field: SerializeField] public float TileBreakDuration { get; private set; }
    [field: SerializeField] public float TileShowDuration { get; private set; }
    [field: SerializeField] public float TileHideDuration { get; private set; }
    [field: SerializeField] public float WinShowDuration { get; private set; }
}