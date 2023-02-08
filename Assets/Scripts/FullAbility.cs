/*using System;
using System.Collections.Generic;

public class FullAbility
{
    private readonly Dictionary<string, ConfirmationAbility> _abilities;

    public List<Basis> this[string key] => _abilities.ContainsKey(key)
        ? _abilities[key].Commands
        : new List<Basis>();


    private FullAbility()
    {
        _abilities = new Dictionary<string, Ability>();
    }

    public static FullAbility Create => new FullAbility();

    public FullAbility On(Ability trigger, ConfirmationAbility ability)
    {
        if (!trigger.IsTrigger || ability.IsTrigger)
            throw new ArgumentException();
        _abilities[trigger.AbilityString] = ability;
        return this;
    }

    public FullAbility OnDeploy(ConfirmationAbility ability)
    {
        _abilities[Basis.Deploy.ToString()] = ability;
        return this;
    }
}*/