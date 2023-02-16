using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using StateMachine.Arguments;
using StateMachine.UpdatedStates;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameEngine
{
    #region Fields

    private Dictionary<Vector2Int, TileInfo> _field;
    private int _turnCounter;

    private readonly Vector2Int _fieldSize;
    private Player _currentPlayer;

    #endregion

    #region Properties

    private bool PlayerTurn => _turnCounter % 2 == 0;
    private bool BotTurn => _turnCounter % 2 == 1;
    public Game Game { get; }
    public RealPlayer Player { get; }
    private Bot Bot { get; }
    private Vector2Int _playedPosition;
    private Goal _completedGoal;

    private IState _currentState;

    #endregion

    #region Constructors

    public GameEngine(Game game, Vector2Int fieldSize)
    {
        Game = game;
        _fieldSize = fieldSize;
        Player = new RealPlayer(this,
            "Player",
            new List<Goal>
            {
                new Goal(Tribe.Magpie, Vector2Int.right),
                new Goal(Tribe.Magpie, Vector2Int.up, Vector2Int.up + Vector2Int.up),
                new Goal(Tribe.Magpie, Vector2Int.one, Vector2Int.one + Vector2Int.one),
            },
            Game.CardConfig.StartDeck);
        Game.GoalManager.SavePlayerGoals(Player.GetGoals);
        Bot = new Bot(this,
            "Bot",
            new List<Goal> {new Goal(Tribe.Magpie, Vector2Int.up)},
            Game.CardConfig.BotDeck);
    }

    #endregion

    #region Methods

    private IEnumerator Init()
    {
        _turnCounter = Random.Range(0, 2);
        yield return InitHand();
        yield return InitField();
    }

    private IEnumerator InitHand()
    {
        for (var i = 0; i < Game.CardConfig.StartCardsCount; i++)
            yield return Player.DrawCard();
        for (var i = 0; i < Game.CardConfig.StartCardsCount; i++)
            yield return Bot.DrawCard();
    }


    private IEnumerator InitField()
    {
        _field = new Dictionary<Vector2Int, TileInfo>();
        for (var i = -_fieldSize.x / 2; i < -_fieldSize.x / 2 + _fieldSize.x; i++)
        for (var j = -_fieldSize.y / 2; j < -_fieldSize.y / 2 + _fieldSize.y; j++)
            _field[new Vector2Int(i, j)] = TileInfo.FreeTile;

        yield return Game.Field.Show(_field);
    }


    private void Win(Player player)
    {
        Game.Win(player, _completedGoal);
    }

    #endregion

    #region IEnumerators

    public IEnumerator Play()
    {
        yield return Init();
        while (true)
        {
            if (PlayerTurn && CheckWin(Bot) || CheckWin(Player) || BotTurn && CheckWin(Bot))
                yield break;
            _currentPlayer = PlayerTurn ? Player : (Player) Bot;
            Game.Messenger.Alert($"Ходит {_currentPlayer.Name}!");
            yield return _currentPlayer.MakeTurn();
            _turnCounter++;
        }
    }

    private bool CheckWin(Player player)
    {
        _completedGoal = player.CompletedGoal(_field);
        var result = _completedGoal != null;
        if (result)
            Win(player);
        return result;
    }

    #endregion

    public IEnumerator PlaceCard(CardMovement card, Vector2Int position)
    {
        _playedPosition = position;
        Game.Field.PlaceCard(card, position);
        var cardData = card.Card.Data;
        _field[position] = TileInfo.Create(cardData.Tribe, cardData.Id);
        _currentState = new ApplicationState();
        yield return ApplyFullAbility(cardData);
    }

    public IEnumerator PlayBotCard(CardData data)
    {
        var freeTiles = _field.Keys.Where(p => _field[p].Free).ToList();
        if (freeTiles.Count == 0)
            yield break;
        var freePos = freeTiles.Choose();
        yield return Game.Field.CreateAndPlaceCard(Bot, data, freePos, false);
        _field[freePos] = TileInfo.Create(data.Tribe, data.Id);
        _currentState = new ApplicationState();
        yield return ApplyFullAbility(data, true);
    }

    private IEnumerator ApplyFullAbility(CardData data, bool isBot = false)
    {
        if (isBot)
            yield return new WaitForSeconds(Game.BotConfig.ThinkRange);
        foreach (var b in data.Bytecode)
        {
            var correctByte = b;
            if (isBot && b == (byte) BytecodeBasis.Confirm)
            {
                correctByte = (byte) BytecodeBasis.ConfirmRandom;
                yield return new WaitForSeconds(Game.BotConfig.ThinkRange);
            }

            _currentState = _currentState.NextState(correctByte);
            if (b.Bb().CommandType() != CommandType.Application)
                continue;
            yield return ApplyState(correctByte.To<ApplicationType>());
            _currentState = new ApplicationState();
            Player.Reset();
        }

        Player.Reset();
    }

    private IEnumerator ApplyState(ApplicationType t)
    {
        var state = (ApplicationState) _currentState;
        yield return HandleAbility(state.Argument, t);
    }

    private IEnumerator HandleAbility(IUniversalArgument arg, ApplicationType t)
    {
        switch (arg.Type)
        {
            case ArgumentType.Field:
                yield return ApplyFieldAbility((FieldArgument) arg, t);
                break;
            case ArgumentType.Card:
                yield return t == ApplicationType.ConfirmAuto
                    ? DoCardOperation((CardArgument) arg)
                    : throw new Exception("Card abilities should be confirmed automatically");
                break;
        }
    }

    private IEnumerator DoCardOperation(CardArgument argument)
    {
        // var player = argument.GetAffectedPlayer();
        // var possibleCards = argument.GetPossibleCards(Player, Bot).ToArray();
        // if (possibleCards.Length == 0)
        //     yield break;
        var possibleCards = argument.GetPossibleCards(Player, Bot);
        switch (argument.Operation.To<DeclarationType, CardOperationType>())
        {
            case CardOperationType.Draw:
                yield return _currentPlayer.DrawCard();
                break;
        }
    }

    private IEnumerator ApplyFieldAbility(FieldArgument fieldArgument, ApplicationType applicationType)
    {
        var possibleTiles = fieldArgument.GetPossibleTiles(_playedPosition, _field).ToArray();
        if (possibleTiles.Length == 0)
            yield break;

        var tilesToDo = new List<Vector2Int>();
        switch (applicationType)
        {
            case ApplicationType.Confirm:
                yield return Game.Field.ShowPossibleTiles(possibleTiles);
                yield return Player.Confirm();
                yield return Game.Field.HidePossibleTiles(possibleTiles);
                tilesToDo.Add(Player.TilePosition);
                break;
            case ApplicationType.ConfirmRandom:
                tilesToDo.Add(possibleTiles.Choose());
                break;
            case ApplicationType.ConfirmAuto:
                if (fieldArgument.All)
                    tilesToDo.AddRange(possibleTiles);
                break;
        }

        foreach (var tile in tilesToDo)
            yield return DoFieldOperation(fieldArgument, tile);
    }

    private IEnumerator DoFieldOperation(IUniversalArgument argument, Vector2Int position)
    {
        switch (argument.Operation.To<DeclarationType, FieldOperationType>())
        {
            case FieldOperationType.Spawn:
                var data = Game.CardConfig.AllCards.GetConcreteCard(argument.ConcreteCards[0]);
                _field[position] = TileInfo.Create(data.Tribe, data.Id);
                yield return Game.Field.CreateAndPlaceCard(_currentPlayer, data, position, true);
                break;
            case FieldOperationType.Kill:
                _field[position] = TileInfo.FreeTile;
                yield return Game.Field.Kill(position);
                break;
            case FieldOperationType.Push:
                var pushDelta = position - _playedPosition;
                var pushFinish = position;
                var pushStartTileInfo = _field[position];
                _field[position] = TileInfo.FreeTile;
                while (_field.ContainsKey(pushFinish + pushDelta) && _field[pushFinish + pushDelta].Free)
                    pushFinish += pushDelta;
                _field[pushFinish] = pushStartTileInfo;
                yield return Game.Field.Push(position, pushFinish);
                break;
            case FieldOperationType.Pull:
                var pullDelta = position - _playedPosition;
                pullDelta = new Vector2Int(pullDelta.x > 0 ? 1 : pullDelta.x < 0 ? -1 : 0,
                    pullDelta.y > 0 ? 1 : pullDelta.y < 0 ? -1 : 0);
                var pullFinish = _playedPosition + pullDelta;
                var pullStartTileInfo = _field[position];
                _field[position] = TileInfo.FreeTile;
                _field[pullFinish] = pullStartTileInfo;
                yield return Game.Field.Pull(position, pullFinish);
                break;
            case FieldOperationType.Lock:
                var obstacleData = Game.CardConfig.AllCards.Obstacle;
                _field[position] = TileInfo.Create(obstacleData.Tribe, obstacleData.Id);
                yield return Game.Field.CreateAndPlaceCard(_currentPlayer, obstacleData, position, true);
                break;
            case FieldOperationType.Unlock:
                _field[position] = TileInfo.FreeTile;
                yield return Game.Field.Kill(position);
                break;
            case FieldOperationType.Break:
                _field.Remove(position);
                yield return Game.Field.Break(position);
                break;
            case FieldOperationType.Build:
                _field[position] = TileInfo.FreeTile;
                yield return Game.Field.Build(position);
                break;
        }
    }
}