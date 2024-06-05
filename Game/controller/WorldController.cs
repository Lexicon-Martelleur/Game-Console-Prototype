using Timers = System.Timers;

using Game.Constant;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.World;
using Game.view;
using Game.Model.Events;
using Game.Model.Base;

namespace Game.Controller;

internal class WorldController(
    IWorldView worldView,
    IWorldService worldService,
    FightController fightController)
{
    private bool _gameOver = false;

    private string _additionalMessage = "";

    private readonly object _drawMapLock = new object();

    // Used to capture the synchronization context of the main thread.
    private SynchronizationContext _syncronizationContext =
        SynchronizationContext.Current ??
        new SynchronizationContext();

    internal void Start()
    {
        SynchronizationContext.SetSynchronizationContext(_syncronizationContext);
        worldView.ClearScreen();
        worldService.InitWorld(GetWorldEvents());
        do
        {
            if (worldService.FightingEnemy != null)
            {
                FightExistingEnemy(worldService.FightingEnemy);
            }
            DrawWorldWithLock(worldService.GetWorldSnapShot(), _additionalMessage);
            HandleMoveCommand(worldView.GetCommand());
        } while (!_gameOver);
    }

    private WorldEvents GetWorldEvents()
    {
        return (
            OnWorldTime,
            OnGoal,
            OnGameOver,
            OnFightStart,
            OnGameToken,
            OnFightStop
        );
    }

    // TODO Re-factor to send previous conquered world in event args.
    private void OnGoal(Object? source, WorldEventArgs<IGameEntity> e)
    {
        var isGoalMsg = worldView.GetIsGoalText(e.Data);
        DrawWorldWithLock(worldService.GetWorldSnapShot(), isGoalMsg, true);
    }

    private void OnGameOver(Object? source, WorldEventArgs<IHero> e)
    {
        _gameOver = true;
        var gameOverMsg = worldView.GetGameOverText(e.Data);
        DrawWorldWithLock(worldService.GetWorldSnapShot(), gameOverMsg);
    }

    private void OnFightStart(Object? source, WorldEventArgs<IEnemy> e)
    {
        bool waitForUserInput = true;
        SetupFightInfoState(e.Data, waitForUserInput);
    }

    // (bool IsHeroDead, Game.Model.GameEntity.IHero Hero)
    private void OnFightStop(
        Object? source,
        WorldEventArgs<(bool IsHeroDead, Game.Model.GameEntity.IHero Hero)> e)
    {
        worldView.ClearScreen();
        if (e.Data.IsHeroDead)
        {
            worldService.CloseWorld();
            _gameOver = true;
            _additionalMessage = worldView.GetGameOverText(worldService.Hero);
        }
    }

    private void OnGameToken(Object? source, WorldEventArgs<IDiscoverableArtifact> e)
    {
        var pickedUpTokenMsg = worldView.GetPickedUpTokenText(e.Data);
        _additionalMessage = pickedUpTokenMsg;
    }

    private void OnWorldTime(Object? source, Timers.ElapsedEventArgs e)
    {
        var worldEnemy = worldService.FightingEnemy;
        if (worldEnemy == null)
        {
            DrawWorldWithLock(worldService.GetWorldSnapShot(), _additionalMessage);
        } else
        {
            var worldEnemyEventArgs = new WorldTimeEventArgs<IEnemy>(
                e.SignalTime,
                worldEnemy);
            _syncronizationContext.Post(
                _ => HandleEnemyFightEvent(worldEnemyEventArgs),
                null);
        }
    }

    private void DrawWorldWithLock(WorldMap? map, string msg, bool pause = false)
    {
        lock (_drawMapLock)
        {
            if (map == null)
            {
                _gameOver = true;
                worldView.WriteGameCongratulation();
            } else
            {
                worldView.DrawWorld(worldService, map, msg, pause);
            }
        }
    }

    private void HandleEnemyFightEvent(WorldTimeEventArgs<IEnemy> e)
    {
        if (worldService.FightingEnemy != null)
        {
            bool waitForUserInput = false;
            SetupFightInfoState(worldService.FightingEnemy, waitForUserInput);
        }
    }

    private void FightExistingEnemy(IEnemy enemy)
    {
        fightController.StartFight(worldService.Hero, enemy);
        worldService.InitWorld(GetWorldEvents());
        worldService.RemoveDeadEnemyFromWorld(enemy);
    }

    private void HandleMoveCommand(Move move)
    {
        try
        {
            worldService.MovePlayerToNextPosition(move);
        }
        catch (InvalidOperationException e) 
        {
            _additionalMessage = worldView.GetWarningMessageText(worldService.Hero, e.Message);
        }
    }

    private void SetupFightInfoState(IEnemy enemy, bool waitForUserInput)
    {
        worldService.CloseWorld();
        worldView.WriteFightInfo(worldService, enemy, waitForUserInput);
    }
}
