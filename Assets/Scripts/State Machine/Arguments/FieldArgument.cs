using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace StateMachine.Arguments
{
    public class FieldArgument : IUniversalArgument
    {
        private List<FieldSpecification> _specifications;
        private Tribe _selectedTribe;
        public Declarations Operation { get; }
        public ArgumentType Type { get; }
        public List<ConcreteCards> ConcreteCards { get; }

        public FieldArgument(byte operation, Tribe selectedTribe = Tribe.None, params FieldSpecification[] argsToFeed)
        {
            Operation = (Declarations) operation;
            Type = ArgumentType.Field;
            ConcreteCards = new List<ConcreteCards>();
            _specifications = new List<FieldSpecification>();

            _selectedTribe = selectedTribe;

            foreach (var arg in argsToFeed)
                Feed((byte) arg);
        }

        public void Feed(byte b)
        {
            var argType = ((BytecodeBasis) b).Argument();

            switch (argType)
            {
                case SpecificationType.Tribe:
                    _selectedTribe = (Tribe) b;
                    break;
                case SpecificationType.FieldSpecification:
                    _specifications.Add((FieldSpecification) b);
                    break;
                case SpecificationType.ConcreteCard:
                    ConcreteCards.Add((ConcreteCards) b);
                    break;
                default:
                    throw new Exception($"FieldArgument expects Tribe or FieldSpecification but gets {argType}");
            }
        }

        public IEnumerable<Vector2Int> GetPossibleTiles(Vector2Int playedPosition,
            Dictionary<Vector2Int, TileInfo> field)
        {
            Debug.Log(string.Join(" ", _specifications));

            IEnumerable<Vector2Int> possibleTiles = field.Keys;
            foreach (var spec in _specifications)
            {
                possibleTiles = spec.GetSatisfying(possibleTiles, playedPosition, field);
            }

            /*var possibleTiles = _specifications.Aggregate<FieldSpecification, IEnumerable<Vector2Int>>(field.Keys,
                (current, spec) => current.Where(t => spec.Satisfied(t, playedPosition, field)));
            */
            possibleTiles = possibleTiles.Where(t =>
                !field.ContainsKey(t) || _selectedTribe.Satisfied(field[t].OccupantTribe)); //field[t].OccupantTribe == 
            return possibleTiles;
        }
    }
}