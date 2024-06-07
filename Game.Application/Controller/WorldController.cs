using Timers = System.Timers;

using Game.Model.GameEntity;
using Game.Model.World;
using Game.Application.View;
using Game.Model.Events;
using Game.Model.Base;


namespace Game.Application.Controller;

internal class WorldController : IWorldController
{
    private bool _gameOver = false;

    private string _additionalMessage = "";

    private readonly object _drawMapLock = new object();
    
    private readonly SynchronizationContext _syncronizationContext;
    
    private readonly IWorldView _worldView;
    
    private readonly IWorldService _worldService;

    public WorldController(
        SynchronizationContext synchronizationContext,
        IWorldView worldView,
        IWorldService worldService)
    {
        _syncronizationContext = synchronizationContext;
        _worldView = worldView;
        _worldService = worldService;
        SubscribedWorldEvents = (
            OnWorldTime,
            OnGoal,
            OnNewWorld,
            OnGameOver,
            OnFightStart,
            OnGameToken,
            OnFightStop,
            OnInvalidMove
        );
    }

    public WorldEvents SubscribedWorldEvents { get; set; }

    public void OnGoal(object? source, WorldEventArgs<IGameEntity> e)
    {
        var isGoalMsg = _worldView.GetIsGoalText(e.Data);
        _additionalMessage = isGoalMsg;
        DrawWorld(true);
    }

    public void OnNewWorld(
        Object? source,
        WorldEventArgs<(IWorld PrevWorld, IWorld NewWorld)> e)
    {
        _additionalMessage = _worldView.GetNewWorldText(
            e.Data.PrevWorld,
            e.Data.NewWorld);
        _worldService.InitWorld(SubscribedWorldEvents);
        DrawWorld();
    }

    public void OnGameOver(object? source, WorldEventArgs<IHero> e)
    {
        _worldService.CloseWorld();
        _gameOver = true;
        var gameOverMsg = _worldView.GetGameOverText(e.Data);
        DrawWorld();
    }

    public void OnFightStart(object? source, WorldEventArgs<IEnemy> e)
    {
        _worldService.CloseWorld();
        bool waitForUserInput = true;
        SetupFightInfoState(e.Data, waitForUserInput);
    }

    public void SetupFightInfoState(IEnemy enemy, bool waitForUserInput)
    {
        _worldService.CloseWorld();
        _worldView.WriteFightInfo(
                _worldService,
                enemy,
                waitForUserInput
        );
    }

    public void OnFightStop(
        Object? source,
        WorldEventArgs<(bool IsHeroDead, IHero Hero)> e)
    {
        _worldView.ClearScreen();
        if (e.Data.IsHeroDead)
        {
            _worldService.CloseWorld();
            _gameOver = true;
            _additionalMessage = _worldView.GetGameOverText(_worldService.Hero);
        }
    }

    public void OnGameToken(object? source, WorldEventArgs<IDiscoverableArtifact> e)
    {
        var pickedUpTokenMsg = _worldView.GetPickedUpTokenText(e.Data);
        _additionalMessage = pickedUpTokenMsg;
    }

    public void OnInvalidMove(object? source, WorldEventArgs<Position> e)
    {
        _additionalMessage = _worldView.GetInvalidMoveText(_worldService.Hero, e.Data);
    }

    public void OnWorldTime(object? source, Timers.ElapsedEventArgs e)
    {
        _syncronizationContext.Send(HandleOnWorldTime, null);
    }

    private void HandleOnWorldTime(object? state)
    {
        var fightingEnemy = _worldService.FightingEnemy;
        if (fightingEnemy == null)
        {
            DrawWorld();
        }
        else
        {
            SetupFightInfoState(fightingEnemy, false);
        }
    }

    public void InitWorld()
    {
        _worldService.InitWorld(SubscribedWorldEvents);
    }

    public bool IsFightingEnemy(out IEnemy? enemy)
    {
        enemy = _worldService.FightingEnemy;
        return enemy != null;
    }

    public IEnemy? GetFightingEnemy()
    {
        return _worldService.FightingEnemy;
    }

    public bool IsGameOver()
    {
        return _gameOver;
    }

    public void DrawWorld(bool pause = false)
    {
        lock (_drawMapLock)
        {
            var map = _worldService.GetWorldSnapShot();
            if (map == null)
            {
                _gameOver = true;
                _worldView.WriteGameCongratulation();
                _worldService.CloseWorld();
            }
            else if (_worldService.Hero.Health == 0)
            {
                _gameOver = true;
                var msg = _worldView.GetGameOverText(_worldService.Hero);
                _worldView.DrawWorld(_worldService, map, msg, pause);
                _worldService.CloseWorld();
            }
            else
            {
                var msg = _additionalMessage;
                _worldView.DrawWorld(_worldService, map, msg, pause);
            }
        }
    }

    public void FightExistingEnemy(IEnemy? enemy, Action<IHero, IEnemy> startFight)
    {
        if (enemy == null)
        {
            return;
        }
        _worldService.CloseWorld();
        startFight(_worldService.Hero, enemy);
        _worldService.InitWorld(SubscribedWorldEvents);
        _worldService.RemoveDeadCreatures(enemy);
    }

    public void HandleMoveCommand()
    {
        try {
            DrawWorld();
            var move = _worldView.GetCommand();
             _worldService.MoveHeroToNextPosition(move);
        } catch { }
    }
}
