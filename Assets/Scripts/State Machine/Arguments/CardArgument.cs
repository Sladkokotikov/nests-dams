using System;
using System.Collections.Generic;
using Enums;

namespace StateMachine.Arguments
{
    public class CardArgument : IUniversalArgument
    {
        private Tribe _selectedTribe;
        private CardSource _selectedSource;
        public Declarations Operation { get; }
        public ArgumentType Type { get; }
        public List<ConcreteCards> ConcreteCards { get; }

        public CardArgument(byte operation, params byte[] argsToFeed)
        {
            Operation = (Declarations) operation;
            Type = ArgumentType.Card;
            ConcreteCards = new List<ConcreteCards>();
            foreach (var arg in argsToFeed)
                Feed(arg);
        }

        public IEnumerable<CardData> GetPossibleCards(Player player, Player bot)
        {
            throw new NotImplementedException();
        }

        public void Feed(byte b)
        {
            var argType = ((BytecodeBasis) b).Argument();
            switch (argType)
            {
                case SpecificationType.Tribe:
                    _selectedTribe = (Tribe) b;
                    break;
                case SpecificationType.CardSource:
                    _selectedSource = (CardSource) b;
                    break;
                case SpecificationType.ConcreteCard:
                    ConcreteCards.Add((ConcreteCards) b);
                    break;
                default:
                    throw new Exception($"CardArgument expects Tribe or CardSource, but gets {argType}");
            }
        }

        
    }
}