using System;
using UnityEngine;

public static class PrefabManager
{
    private static readonly Lazy<Tile> Tile = new Lazy<Tile>(() => Resources.Load<Tile>("Prefabs/TilePrefab"));
    public static Tile TilePrefab => Tile.Value;
    
    private static readonly Lazy<Card> Card = new Lazy<Card>(() => Resources.Load<Card>("Prefabs/CardPrefab"));
    public static Card CardPrefab => Card.Value;
    
    private static readonly Lazy<RectTransform> Empty = new Lazy<RectTransform>(() => Resources.Load<RectTransform>("Prefabs/EmptyPrefab"));
    public static RectTransform EmptyPrefab => Empty.Value;
    
    
    
    /*private static readonly Lazy<Sprite> CommonMagpie = new Lazy<Sprite>(() => Resources.Load<Sprite>("Sprites/CommonMagpie"));
    public static Sprite CommonMagpieSprite => CommonMagpie.Value;*/
}
