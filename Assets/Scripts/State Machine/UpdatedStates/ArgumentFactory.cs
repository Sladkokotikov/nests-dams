using System;
using Enums;
using StateMachine.Arguments;

namespace StateMachine.UpdatedStates
{
    public static class ArgumentFactory
    {
        public static IUniversalArgument Create(byte b)
        {
            IUniversalArgument result = (Declarations) b switch
            {
                Declarations.Spawn => new FieldArgument(b),
                Declarations.Kill => new FieldArgument(b, Tribe.Playable),
                Declarations.Push => new FieldArgument(b, Tribe.Playable, FieldSpecification.Surrounding),
                Declarations.Pull => new FieldArgument(b, Tribe.Playable, FieldSpecification.Plus),
                Declarations.Draw => new CardArgument(b),
                Declarations.Lock => new FieldArgument(b),
                Declarations.Unlock => new FieldArgument(b, Tribe.Obstacle),
                Declarations.Break => new FieldArgument(b),
                Declarations.Build => new FieldArgument(b, Tribe.None, FieldSpecification.OuterEdge),
                _ => throw new ArgumentOutOfRangeException(nameof(b), b, null)
            };


            return result;
        }
    }
}