using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField]public Tribe Tribe{ get; private set; }

    [field: SerializeField]public string CardName{ get; private set; }
    [field: SerializeField]public string AbilityMask{ get; private set; }

    [field: SerializeField]public Sprite Image{ get; private set; }
    [field: SerializeField]public Sprite Icon{ get; private set; }
    
    public IEnumerable<byte> Bytecode => Commands.Cast<byte>();
    [field: SerializeField]public List<BytecodeBasis> Commands{ get; private set; }
}