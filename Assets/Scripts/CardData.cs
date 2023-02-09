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
    
    public IEnumerable<byte> Bytecode => ShownCommands.Cast<byte>();
    
    public BytecodeBasis Tail =>
        _hiddenCommands.Count > 0 ? _hiddenCommands[_hiddenCommands.Count - 1] : BytecodeBasis.Confirm;

    public IEnumerable<BytecodeBasis> ShownCommands => _hiddenCommands;
    private List<BytecodeBasis> _hiddenCommands;

    public void InitHiddenCommands()
    {
        _hiddenCommands ??= new List<BytecodeBasis>();
    }

    public void AddCommand(BytecodeBasis command)
    {
        _hiddenCommands.Add(command);
    }

    public void Clear()
    {
        _hiddenCommands.Clear();
    }
}