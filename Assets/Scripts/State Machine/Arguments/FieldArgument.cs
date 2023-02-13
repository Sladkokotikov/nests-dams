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

        public FieldArgument(byte operation, Tribe selectedTribe = Tribe.None, params FieldSpecificationType[] argsToFeed)
        {
            Operation = operation.To<DeclarationType>();
            Type = ArgumentType.Field;
            ConcreteCards = new List<ConcreteCard>();
            _specifications = new List<FieldSpecificationType>();

            _selectedTribe = selectedTribe;

            
            foreach (var arg in argsToFeed)
                _specifications.Add(arg);
            
            Debug.Log(string.Join(" ", _specifications));
            Debug.Log(_specifications.Count);
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
                default:
                    throw new Exception($"FieldArgument expects Tribe or FieldSpecification but gets {argType}");
            }
        }

        public IEnumerable<Vector2Int> GetPossibleTiles(Vector2Int playedPosition,
            Dictionary<Vector2Int, TileInfo> field)
        {
            Debug.Log(string.Join(" ", _specifications));
            Debug.Log(_specifications.Count);
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