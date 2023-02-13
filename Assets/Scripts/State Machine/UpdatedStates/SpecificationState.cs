using System;
using Enums;
using StateMachine.Arguments;

namespace StateMachine.UpdatedStates
{
    public class SpecificationState : IState
    {
        private readonly IUniversalArgument _argument;

        public SpecificationState(IUniversalArgument argument, byte b)
        {
            _argument = argument;
            _argument.Feed(b);
        }

        public IState NextState(byte b)
        {
            var type = b.Bb().CommandType();

            return type switch
            {
                CommandType.Specification => new SpecificationState(_argument, b),
                CommandType.Application => new ApplicationState(_argument),
                _ => throw new Exception(
                    $"Specification must be followed by Specification or Application, not by {type}")
            };
        }
    }
}