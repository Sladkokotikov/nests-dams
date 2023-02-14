using System;
using System.Collections.Generic;
using Enums;

namespace StateMachine.Arguments
{
    public class CardArgument : IUniversalArgument
    {
        private Tribe _selectedTribe;
        private CardSourceType _selectedSource;
        public DeclarationType Operation { get; }
        public ArgumentType Type { get; }
        public List<ConcreteCard> ConcreteCards { get; }
        public bool All { get; private set; }

        public CardArgument(byte operation, params byte[] argsToFeed)
        {
            Operation = operation.To<DeclarationType>();
            Type = ArgumentType.Card;
            ConcreteCards = new List<ConcreteCard>();
            foreach (var arg in argsToFeed)
                Feed(arg);
        }

        public IEnumerable<CardData> GetPossibleCards(Player player, Player bot)
        {
            throw new NotImplementedException();
        }

        public void Feed(byte b)
        {
            var specType = b.Bb().SpecificationType();
            switch (specType)
            {
                case SpecificationType.Tribe:
                    _selectedTribe = b.To<Tribe>();
                    break;
                case SpecificationType.CardSource:
                    _selectedSource = b.To<CardSourceType>();
                    break;
                case SpecificationType.ConcreteCard:
                    ConcreteCards.Add(b.To<ConcreteCard>());
                    break;
                case SpecificationType.FieldRule:
                    throw new Exception("FieldRule was fed to Card Argument");
                case SpecificationType.AllControl:
                    All = true;
                    break;
            }
        }
    }
}