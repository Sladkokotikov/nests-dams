using System;
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
            var type = ((BytecodeBasis) b).Type();

            return type switch
            {
                BasisType.Specification => new SpecificationState(_argument, b),
                BasisType.Application => new ApplicationState(_argument),
                _ => throw new Exception(
                    $"Declaration must be followed by Specification or Application, not by {type} ({(BytecodeBasis) b})")
            };
        }
    }
}