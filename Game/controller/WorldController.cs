using Timers = System.Timers;

using Game.constants;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.World;
using Game.view;
using Game.Events;

namespace Game.Controller;

internal class WorldController(
    IWorldView worldView,
    IWorld world,
    FightController fightController)
{
    private bool _gameOver = false;

    private bool _goal = false;

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
        world.InitWorld(OnWorldTimeChange, OnGoal, OnGameOver, OnFight);
        do
        {
            if (world.FightingEnemy != null)
            {
                FightExistingEnemy(world.FightingEnemy);
            }
            DrawWorldWithLock(world.GetWorldSnapShot(), _additionalMessage);
            HandleMoveCommand(worldView.GetCommand());
        } while (!_gameOver && !_goal);
    }

    private void OnGoal(Object? source, WorldEventArgs<IGameEntity> e)
    {
        _goal = true;
        var isGoalMsg = worldView.GetIsGoalText(e.Data);
        DrawWorldWithLock(world.GetWorldSnapShot(), isGoalMsg);
    }

    private void OnGameOver(Object? source, WorldEventArgs<IHero> e)
    {
        _gameOver = true;
        var gameOverMsg = worldView.GetGameOverText(e.Data);
        DrawWorldWithLock(world.GetWorldSnapShot(), gameOverMsg);
    }

    private void OnFight(Object? source, WorldEventArgs<IEnemy> e)
    {
        bool waitForUserInput = true;
        SetupFightInfoState(e.Data, waitForUserInput);
    }

    private void OnWorldTimeChange(Object? source, Timers.ElapsedEventArgs e)
    {
        var worldEnemy = world.FightingEnemy;
        if (worldEnemy == null)
        {
            DrawWorldWithLock(world.GetWorldSnapShot(), _additionalMessage);
        } else
        {
            var worldEnemyEventArgs = new WorldTimeEventArgs<IEnemy>(e.SignalTime, worldEnemy);
            _syncronizationContext.Post(
                _ => HandleEnemyFightEvent(worldEnemyEventArgs),
                null);
        }
    }

    private void DrawWorldWithLock(WorldMap map, string msg)
    {
        lock (_drawMapLock)
        {
            worldView.DrawWorld(world, map, msg);
        }
    }

    private void HandleEnemyFightEvent(WorldTimeEventArgs<IEnemy> e)
    {
        if (world.FightingEnemy != null)
        {
            bool waitForUserInput = false;
            SetupFightInfoState(world.FightingEnemy, waitForUserInput);
        }
    }

    private void FightExistingEnemy(IEnemy enemy)
    {
        fightController.StartFight(world.Hero, enemy);
        if (!world.IsHeroDead())
        {
            world.RemoveFightingEnemyFromWorld(enemy);
            world.InitWorld(OnWorldTimeChange, OnGoal, OnGameOver, OnFight);
            worldView.ClearScreen();
        }
        else
        {
            _gameOver = true;
            var gameOverMsg = worldView.GetGameOverText(world.Hero);
            DrawWorldWithLock(world.GetWorldSnapShot(), gameOverMsg);
        }
    }

    private void HandleMoveCommand(Move move)
    {
        try
        {
            world.MovePlayerToNextPosition(move);
        }
        catch (InvalidOperationException e) 
        {
            _additionalMessage = worldView.GetWarningMessageText(world.Hero, e.Message);
        }
    }

    private void SetupFightInfoState(IEnemy enemy, bool waitForUserInput)
    {
        world.CloseWorld();
        worldView.WriteFightInfo(world, enemy, waitForUserInput);
    }
}
