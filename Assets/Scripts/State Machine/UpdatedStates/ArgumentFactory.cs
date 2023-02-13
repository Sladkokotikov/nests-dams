using System;
using Enums;
using StateMachine.Arguments;

namespace StateMachine.UpdatedStates
{
    public static class ArgumentFactory
    {
        public static IUniversalArgument Create(byte b)
        {
            IUniversalArgument result = b.Bb().DeclarationType() switch
            {
                DeclarationType.Spawn => new FieldArgument(b),
                DeclarationType.Kill => new FieldArgument(b, Tribe.Playable),
                DeclarationType.Push => new FieldArgument(b, Tribe.Playable, FieldSpecificationType.Surrounding),
                DeclarationType.Pull => new FieldArgument(b, Tribe.Playable, FieldSpecificationType.Plus),
                DeclarationType.Draw => new CardArgument(b),
                DeclarationType.Lock => new FieldArgument(b),
                DeclarationType.Unlock => new FieldArgument(b, Tribe.Obstacle),
                DeclarationType.Break => new FieldArgument(b),
                DeclarationType.Build => new FieldArgument(b, Tribe.None, FieldSpecificationType.OuterEdge),
                _ => throw new ArgumentOutOfRangeException(nameof(b), b, null)
            };


            return result;
        }
    }
}