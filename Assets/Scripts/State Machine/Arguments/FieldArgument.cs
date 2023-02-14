using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace StateMachine.Arguments
{
    public class FieldArgument : IUniversalArgument
    {
        private readonly List<FieldSpecificationType> _specifications;
        private Tribe _selectedTribe;
        public DeclarationType Operation { get; }
        public ArgumentType Type { get; }
        public List<ConcreteCard> ConcreteCards { get; }
        public bool All { get; private set; }

        public FieldArgument(byte operation, Tribe selectedTribe = Tribe.None,
            params FieldSpecificationType[] argsToFeed)
        {
            Operation = operation.To<DeclarationType>();
            Type = ArgumentType.Field;
            ConcreteCards = new List<ConcreteCard>();
            _specifications = new List<FieldSpecificationType>();
            _selectedTribe = selectedTribe;
            
            foreach (var arg in argsToFeed)
                _specifications.Add(arg);
        }

        public void Feed(byte b)
        {
            var argType = b.Bb().SpecificationType();

            switch (argType)
            {
                case SpecificationType.Tribe:
                    _selectedTribe = b.To<Tribe>();
                    break;
                case SpecificationType.FieldRule:
                    _specifications.Add(b.To<FieldSpecificationType>());
                    break;
                case SpecificationType.ConcreteCard:
                    ConcreteCards.Add(b.To<ConcreteCard>());
                    break;
                case SpecificationType.AllControl:
                    All = true;
                    break;
                case SpecificationType.CardSource:
                    throw new Exception("Field argument was fed with CardSource");
            }
        }

        public IEnumerable<Vector2Int> GetPossibleTiles(Vector2Int playedPosition,
            Dictionary<Vector2Int, TileInfo> field)
        {
            var possibleTiles = _specifications
                .Aggregate<FieldSpecificationType, IEnumerable<Vector2Int>>
                (field.Keys,
                    (current, spec) => spec.GetSatisfying(current, playedPosition, field));

            possibleTiles = possibleTiles.Where(t =>
                !field.ContainsKey(t) || _selectedTribe.Satisfied(field[t].OccupantTribe));
            return possibleTiles;
        }
    }
}