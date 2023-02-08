
using System.Collections.Generic;

public class Ability
{
    public bool IsTrigger { get; }
    public readonly List<Basis> Commands;
    public string AbilityString { get; }

    public Ability(List<Basis> commands, string code, bool isTrigger)
    {
        Commands = commands;
        AbilityString = code;
        IsTrigger = isTrigger;
    }
}