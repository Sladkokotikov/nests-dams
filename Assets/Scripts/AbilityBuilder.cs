/*using System;
using System.Collections.Generic;
using System.Linq;

public class AbilityBuilder
{
    /*private int _state;

    private readonly List<Basis> _commands;

    private readonly bool _isTrigger;

    // 0 = Confirm
    // 1 = Action
    // 2 = Criteria

    private AbilityBuilder(bool isTrigger)
    {
        _state = 0;
        _isTrigger = isTrigger;
        _commands = new List<Basis>();
    }

    public static AbilityBuilder Trigger => new AbilityBuilder(true);
    public static AbilityBuilder Executable => new AbilityBuilder(false);

    public AbilityBuilder Add(Basis command)
    {
        var commandState = command.State();
        if (!CanMakeTransition(_state, commandState) || _isTrigger && commandState == 0)
            throw new ArgumentException(
                $"Can't add {command} to the ability. Last command is {_commands[_commands.Count - 1]} and ability is trigger? -> {_isTrigger}");
        _state = command.State();
        _commands.Add(command);
        return this;
    }

    public Ability Build()
    {
        if (!_isTrigger && _commands[_commands.Count - 1].State() != 0)
            throw new ArgumentException();
        
        return new Ability(_commands,
            string.Join(" ", _commands.Select(c => c.ToString())),
            _isTrigger);
    }


    private static bool CanMakeTransition(int from, int to)
        => from switch
        {
            0 => to == 1,
            1 => to != 1,
            2 => to != 1,
            _ => throw new ArgumentOutOfRangeException(nameof(@from), @from, null)
        };#1#
}*/