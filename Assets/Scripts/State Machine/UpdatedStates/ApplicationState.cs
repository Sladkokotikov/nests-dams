using System;
using Enums;
using StateMachine.Arguments;

namespace StateMachine.UpdatedStates
{
    public class ApplicationState : IState
    {
        public IUniversalArgument Argument { get; }

        public ApplicationState()
        {
            
        }
        public ApplicationState(IUniversalArgument arg)
        {
            Argument = arg;
        }

        public IState NextState(byte b)
        {
            var type = ((BytecodeBasis) b).Type();

            return type switch
            {
                BasisType.Declaration => new DeclarationState(b),
                _ => throw new Exception($"Application must be followed by Declaration, not by {type}")
            };
        }
    }
}