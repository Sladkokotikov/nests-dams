using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public Tribe Tribe { get; private set; }
    
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string AbilityMask { get; private set; }

    [field: SerializeField] public byte[] Bytecode { get; private set; }

    public void Load(int id, Tribe tribe, string cardName, string mask, ConfirmationAbility ability)
    {
        ID = id;
        Tribe = tribe;
        Name = cardName;
        AbilityMask = mask;
        Bytecode = ability.Bytecode.ToArray();
    }
}