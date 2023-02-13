using System.Collections.Generic;
using Enums;

namespace StateMachine.Arguments
{
    public interface IUniversalArgument
    {
        void Feed(byte b);
        Declarations Operation { get; }
        ArgumentType Type { get; }
        List<ConcreteCards> ConcreteCards { get; }
    }
}