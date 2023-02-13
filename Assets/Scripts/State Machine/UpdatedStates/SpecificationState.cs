using System;
using StateMachine.Arguments;

namespace StateMachine.UpdatedStates
{
    public class SpecificationState : IState
    {
        private IUniversalArgument _argument;

        public SpecificationState(IUniversalArgument argument, byte b)
        {
            _argument = argument;
            _argument.Feed(b);
        }

        public IState NextState(byte b)
        {
            var type = ((BytecodeBasis) b).Type();

            return type switch
            {
                BasisType.Specification => new SpecificationState(_argument, b),
                BasisType.Application => new ApplicationState(_argument),
                _ => throw new Exception(
                    $"Specification must be followed by Specification or Application, not by {type}")
            };
        }
    }
}