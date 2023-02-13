using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{
    public int id;
    public Tribe tribe;

    public string cardName;
    public string abilityMask;

    public Sprite image;
    public Sprite icon;
    
    public IEnumerable<byte> Bytecode => commands.Cast<byte>();
    public List<BytecodeBasis> commands;
}