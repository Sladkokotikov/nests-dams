using System.Collections.Generic;
using Enums;

namespace StateMachine.Arguments
{
    public interface IUniversalArgument
    {
        void Feed(byte b);
        DeclarationType Operation { get; }
        ArgumentType Type { get; }
        List<ConcreteCard> ConcreteCards { get; }
        bool All { get; }
    }
}