using UnityEngine;

public class ConfigurationManager : MonoBehaviour
{
    [field: SerializeField] public int TileWidth { get; private set; }
    [SerializeField] private int tileWidthOffsetMin;
    [SerializeField] private int tileWidthOffsetMax;
    public int TileOffsetRange => Random.Range(tileWidthOffsetMin, tileWidthOffsetMax);
    [field: SerializeField] public Vector2Int FieldSize { get; private set; }

    [SerializeField] private float botThinkTimeMin;
    [SerializeField] private float botThinkTimeMax;
    public float BotThinkTimeRange => Random.Range(botThinkTimeMin, botThinkTimeMax);


    public readonly int Fade = Shader.PropertyToID("_Fade");
    public readonly int Glow = Shader.PropertyToID("_Glow");
    public readonly int Scale = Shader.PropertyToID("_Scale");
}