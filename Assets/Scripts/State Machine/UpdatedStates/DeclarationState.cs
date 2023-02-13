using System;
using Enums;
using StateMachine.Arguments;

namespace StateMachine.UpdatedStates
{
    public class DeclarationState : IState
    {
        private readonly IUniversalArgument _argument;

        public DeclarationState(byte b)
        {
            _argument = ArgumentFactory.Create(b);
        }

        public IState NextState(byte b)
        {
            var type = b.Bb().CommandType();

            return type switch
            {
                CommandType.Specification => new SpecificationState(_argument, b),
                CommandType.Application => new ApplicationState(_argument),
                _ => throw new Exception(
                    $"Declaration must be followed by Specification or Application, not by {type} ({b.Bb()})")
            };
        }
    }
}