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
    private Dictionary<byte, Func<IEnumerator>> _processes;
    private Dictionary<byte, Func<Vector2Int, IEnumerator>> _appliers;
    private Player _currentPlayer;

    #endregion

    #region Properties

    private bool PlayerTurn => _turnCounter % 2 == 0;
    private bool BotTurn => _turnCounter % 2 == 1;
    public Game Game { get; }
    public RealPlayer Player { get; }
    private Bot Bot { get; }
    private byte _operation;
    private IEnumerable<Vector2Int> _possibleSelections;
    private Vector2Int _playedPosition;
    private Goal _completedGoal;
    private bool _waitForStoreTribe;
    private Tribe _storedTribe;

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
            new List<CardData>
            {
                ServiceLocator.Locator.CardManager.GetCard(0),
                ServiceLocator.Locator.CardManager.GetCard(1),
                ServiceLocator.Locator.CardManager.GetCard(2),
                ServiceLocator.Locator.CardManager.GetCard(3),
                ServiceLocator.Locator.CardManager.GetCard(4),
                ServiceLocator.Locator.CardManager.GetCard(5),
            });
        Game.SavePlayerGoals(Player.GetGoals);
        Bot = new Bot(this,
            "Bot",
            new List<Goal> {new Goal(Tribe.Magpie, Vector2Int.up)},
            new List<CardData>
            {
                //ServiceLocator.Locator.CardManager.GetCard(0),
                // ServiceLocator.Locator.CardManager.GetCard(1),
                // ServiceLocator.Locator.CardManager.GetCard(2),
                // ServiceLocator.Locator.CardManager.GetCard(3),
                //ServiceLocator.Locator.CardManager.GetCard(4),
                //ServiceLocator.Locator.CardManager.GetCard(5),
            });
    }

    #endregion


    #region Methods

    private IEnumerator Init()
    {
        //EventManager.AbilityApply += HandleAbility;
        _turnCounter = Random.Range(0, 2);
        InitProcesses();
        yield return InitHand();
        yield return InitField();
    }

    private void InitProcesses()
    {
        _processes = new Dictionary<byte, Func<IEnumerator>>
        {
            [(byte) BytecodeBasis.Build] = StoreBuild,
            [(byte) BytecodeBasis.Break] = StoreBreak,
            [(byte) BytecodeBasis.Confirm] = Confirm,
            [(byte) BytecodeBasis.ConfirmRandom] = ConfirmRandom,
            [(byte) BytecodeBasis.ConfirmAuto] = ConfirmAuto,
            [(byte) BytecodeBasis.Draw] = StoreDraw,
            [(byte) BytecodeBasis.Kill] = StoreKill,
            [(byte) BytecodeBasis.Spawn] = StoreSpawn,
            [(byte) BytecodeBasis.Push] = StorePush,
            [(byte) BytecodeBasis.Pull] = StorePull,


            [(byte) BytecodeBasis.Adjacent] = () => Select(v => v.IsAdjacent(_playedPosition)),
            [(byte) BytecodeBasis.Surrounding] = () => Select(v => v.IsSurrounding(_playedPosition)),
            [(byte) BytecodeBasis.Edge] = () => Select(v => v.IsOnEdge(_field)),

            [(byte) BytecodeBasis.Magpie] = () =>
            {
                if (!_waitForStoreTribe)
                    return Select(v => !_field[v].Occupied || _field[v].OccupantTribe == Tribe.Magpie);
                _storedTribe = Tribe.Magpie;
                _waitForStoreTribe = false;
                return null;
            },
            [(byte) BytecodeBasis.Beaver] = () =>
            {
                if (!_waitForStoreTribe)
                    return Select(v => !_field[v].Occupied || _field[v].OccupantTribe == Tribe.Beaver);
                _storedTribe = Tribe.Beaver;
                _waitForStoreTribe = false;
                return null;
            },
        };

        _appliers = new Dictionary<byte, Func<Vector2Int, IEnumerator>>
        {
            [(byte) BytecodeBasis.Build] = ApplyBuild,
            [(byte) BytecodeBasis.Break] = ApplyBreak,
            [(byte) BytecodeBasis.Draw] = ApplyDraw,
            [(byte) BytecodeBasis.Kill] = ApplyKill,
            [(byte) BytecodeBasis.Spawn] = ApplySpawn,
            [(byte) BytecodeBasis.Push] = ApplyPush,
            [(byte) BytecodeBasis.Pull] = ApplyPull,
        };
    }

    private IEnumerator ApplyPull(Vector2Int position)
    {
        var delta = position - _playedPosition;
        delta = new Vector2Int(delta.x > 0 ? 1 : delta.x < 0 ? -1 : 0, delta.y > 0 ? 1 : delta.y < 0 ? -1 : 0);
        var finish = _playedPosition + delta;
        var startTileInfo = _field[position];
        _field[position] = TileInfo.FreeTile;
        _field[finish] = startTileInfo;
        yield return Game.Pull(position, finish);
    }

    private IEnumerator StorePull()
    {
        _operation = (byte) BytecodeBasis.Pull;
        _possibleSelections = GetPullTargets();
        yield break;
    }

    private IEnumerable<Vector2Int> GetPullTargets()
    {
        var lastRaycastDirection = -Vector2Int.one;
        var nextDirectionDelta = Vector2Int.left;
        var result = new List<Vector2Int>();
        for (var i = 0; i < 8; i++)
        {
            if (i % 2 == 0)
                nextDirectionDelta = new Vector2Int(nextDirectionDelta.y, -nextDirectionDelta.x);
            var currPos = _playedPosition;
            while (_field.ContainsKey(currPos + lastRaycastDirection))
            {
                currPos += lastRaycastDirection;
                if (!_field[currPos].Occupied)
                    continue;
                result.Add(currPos);
                break;
            }

            lastRaycastDirection += nextDirectionDelta;
        }

        return result;
    }

    private IEnumerator ApplyPush(Vector2Int position)
    {
        var delta = position - _playedPosition;
        var finish = position;
        var startTileInfo = _field[position];
        _field[position] = TileInfo.FreeTile;
        while (_field.ContainsKey(finish + delta) && _field[finish + delta].Free)
            finish += delta;
        _field[finish] = startTileInfo;
        yield return Game.Push(position, finish);
    }

    private IEnumerator StorePush()
    {
        _operation = (byte) BytecodeBasis.Push;
        _possibleSelections = _field.Keys.Where(v => v.IsSurrounding(_playedPosition) && _field[v].Occupied);
        yield break;
    }

    private IEnumerator ApplySpawn(Vector2Int position)
    {
        var data = GetCommonCard(_storedTribe);
        _field[position] = TileInfo.Create(_storedTribe, data.id);
        yield return Game.CreateAndPlaceCard(data, position, true);
    }

    private static CardData GetCommonCard(Tribe storedTribe)
        => storedTribe switch
        {
            Tribe.Beaver => ServiceLocator.Locator.CardManager.GetCard(1),
            /*Tribe.Magpie => expr,
            Tribe.Obstacle => expr,
            Tribe.Playable => expr,
            Tribe.Any => expr,
            Tribe.None => expr,*/
            _ => throw new ArgumentOutOfRangeException(nameof(storedTribe), storedTribe, null)
        };

    private IEnumerator StoreSpawn()
    {
        _operation = (byte) BytecodeBasis.Spawn;
        _possibleSelections = _field.Keys.Where(p => !_field[p].Occupied);
        _waitForStoreTribe = true;
        yield return null;
    }

    private IEnumerator Select(Func<Vector2Int, bool> criteria)
    {
        _possibleSelections = _possibleSelections.Where(criteria);
        //Debug.Log(_possibleSelections.Count());
        yield return null;
    }

    private IEnumerator ApplyDraw(Vector2Int arg)
    {
        yield return _currentPlayer.DrawCard();
    }

    private IEnumerator StoreDraw()
    {
        _operation = (byte) BytecodeBasis.Draw;
        yield break;
    }

    private IEnumerator ConfirmAuto()
    {
        yield return ApplyOperation(Vector2Int.zero);
        Player.Reset();
    }

    private IEnumerator ApplyKill(Vector2Int position)
    {
        _field[position] = TileInfo.FreeTile;
        yield return Game.Kill(position);
    }

    private IEnumerator StoreKill()
    {
        _operation = (byte) BytecodeBasis.Kill;
        _possibleSelections = _field.Keys.Where(p => _field[p].Occupied);
        yield return null;
    }

    private IEnumerator ApplyBreak(Vector2Int position)
    {
        _field.Remove(position);
        yield return Game.Break(position);
    }

    private IEnumerator StoreBreak()
    {
        _operation = (byte) BytecodeBasis.Break;
        _possibleSelections = _field.Keys.Where(p => _field[p].Free);
        yield return null;
    }

    private IEnumerator Confirm()
    {
        if (_possibleSelections.Any())
        {
            Game.ShowPossibleTiles(_possibleSelections);
            yield return Player.Confirm();
            Game.HidePossibleTiles(_possibleSelections);
            if (Player.TilePosition != null)
                yield return ApplyOperation(Player.TilePosition);
        }

        Clear();
        Player.Reset();
    }

    private IEnumerator ConfirmRandom()
    {
        if (_possibleSelections.Any())
            yield return ApplyOperation(_possibleSelections.Choose());
        Player.Reset();
    }

    private IEnumerator ApplyOperation(Vector2Int selectedTile)
    {
        yield return _appliers[_operation](selectedTile);
    }

    private IEnumerator StoreBuild()
    {
        _operation = (byte) BytecodeBasis.Build;
        _possibleSelections = _field.Keys.SelectMany(v => v.Adjacent()).Where(v => !_field.ContainsKey(v));
        yield return null;
    }

    private IEnumerator ApplyBuild(Vector2Int position)
    {
        _field[position] = TileInfo.FreeTile;
        yield return Game.Build(position);
    }

    private IEnumerator InitHand()
    {
        for (var i = 0; i < 3; i++)
            yield return Player.DrawCard();
        for (var i = 0; i < 4; i++)
            yield return Bot.DrawCard();
    }


    private IEnumerator InitField()
    {
        _field = new Dictionary<Vector2Int, TileInfo>();
        for (var i = -_fieldSize.x / 2; i < -_fieldSize.x / 2 + _fieldSize.x; i++)
        for (var j = -_fieldSize.y / 2; j < -_fieldSize.y / 2 + _fieldSize.y; j++)
            _field[new Vector2Int(i, j)] = TileInfo.FreeTile;


        yield return Game.ShowField(_field);
    }


    private void Win(Player player)
    {
        //GameEnded = true;
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
            Game.Alert($"Ходит {_currentPlayer.Name}!");
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

    public IEnumerator PlaceCard(Card card, Vector2Int position)
    {
        _playedPosition = position;
        Game.PlaceCard(card, position);
        _field[position] = TileInfo.Create(card.Data.tribe, card.Data.id);
        _currentState = new ApplicationState();
        yield return ApplyFullAbility(card.Data);
    }

    public IEnumerator PlayBotCard(CardData card)
    {
        var freeTiles = _field.Keys.Where(p => _field[p].Free).ToList();
        if (freeTiles.Count == 0)
            yield break;
        var freePos = freeTiles.Choose();
        yield return Game.CreateAndPlaceCard(card, freePos, false);
        _field[freePos] = TileInfo.Create(card.tribe, card.id);
        _currentState = new ApplicationState();
        yield return ApplyFullAbility(card, true);
    }

    private IEnumerator ApplyFullAbility(CardData data, bool isBot = false)
    {
        if (isBot)
            yield return new WaitForSeconds(ServiceLocator.Locator.ConfigurationManager.BotThinkTimeRange);
        foreach (var b in data.Bytecode)
        {
            var correctByte = b;
            if (isBot && b == (byte) BytecodeBasis.Confirm)
            {
                correctByte = (byte) BytecodeBasis.ConfirmRandom;
                yield return new WaitForSeconds(ServiceLocator.Locator.ConfigurationManager.BotThinkTimeRange);
            }

            //yield return _processes[correctByte]();
            //Debug.Log(b);

            _currentState = _currentState.NextState(correctByte);
            if (((BytecodeBasis) b).Type() != BasisType.Application) 
                continue;
            yield return ApplyState((ApplicationType)correctByte);
            _currentState = new ApplicationState();

        }

        Clear();
    }

    private void Clear()
    {
        Player.Reset();
        _possibleSelections = null;
    }

    private IEnumerator ApplyState(ApplicationType t)
    {
        var state = (ApplicationState)_currentState;
        yield return HandleAbility(state.Argument, t);
    }



    private IEnumerator HandleAbility(IUniversalArgument arg, ApplicationType t)
    {
        yield return arg.Type switch
        {
            ArgumentType.Field => ApplyFieldAbility((FieldArgument) arg, t),
            ArgumentType.Card => ApplyCardAbility((CardArgument) arg, t),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private IEnumerator ApplyCardAbility(CardArgument cardArgument, ApplicationType applicationType)
    {
        Debug.Log("Card Ability applied");
        yield break;
    }

    private IEnumerator ApplyFieldAbility(FieldArgument fieldArgument, ApplicationType applicationType)
    {
        var possibleTiles = fieldArgument.GetPossibleTiles(_playedPosition, _field).ToArray();
        if (possibleTiles.Length == 0)
            yield break;
        var tile = possibleTiles.Choose();
        
        if(applicationType == ApplicationType.Confirm)
        {
            Game.ShowPossibleTiles(possibleTiles);
            yield return Player.Confirm();
            Game.HidePossibleTiles(possibleTiles);
            tile = Player.TilePosition;
        }
        
        yield return DoOperation(fieldArgument, tile);
    }

    private IEnumerator DoOperation(IUniversalArgument argument, Vector2Int position)
    {
        switch (argument.Operation)
        {
            case Declarations.Spawn:
                var data = ServiceLocator.Locator.CardManager.GetCard(argument.ConcreteCards[0]);
                _field[position] = TileInfo.Create(_storedTribe, data.id);
                yield return Game.CreateAndPlaceCard(data, position, true);
                break;
            case Declarations.Kill:
                _field[position] = TileInfo.FreeTile;
                yield return Game.Kill(position);
                break;
            case Declarations.Push:
                var pushDelta = position - _playedPosition;
                var pushFinish = position;
                var pushStartTileInfo = _field[position];
                _field[position] = TileInfo.FreeTile;
                while (_field.ContainsKey(pushFinish + pushDelta) && _field[pushFinish + pushDelta].Free)
                    pushFinish += pushDelta;
                _field[pushFinish] = pushStartTileInfo;
                yield return Game.Push(position, pushFinish);
                break;
            case Declarations.Pull:
                var pullDelta = position - _playedPosition;
                pullDelta = new Vector2Int(pullDelta.x > 0 ? 1 : pullDelta.x < 0 ? -1 : 0, pullDelta.y > 0 ? 1 : pullDelta.y < 0 ? -1 : 0);
                var pullFinish = _playedPosition + pullDelta;
                var pullStartTileInfo = _field[position];
                _field[position] = TileInfo.FreeTile;
                _field[pullFinish] = pullStartTileInfo;
                yield return Game.Pull(position, pullFinish);
                break;
            case Declarations.Draw:
                yield return _currentPlayer.DrawCard();
                break;
            case Declarations.Lock:
                break;
            case Declarations.Unlock:
                break;
            case Declarations.Break:
                _field.Remove(position);
                yield return Game.Break(position);
                break;
            case Declarations.Build:
                _field[position] = TileInfo.FreeTile;
                yield return Game.Build(position);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(argument.Operation), argument.Operation, null);
        }
    }
}