using Timers = System.Timers;

using Game.constants;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.World;
using Game.view;
using Game.Events;

namespace Game.Controller;

internal class GameController(
    IGameView view,
    IWorldService world,
    FightController fightController)
{
    private bool _gameOver = false;

    private bool _goal = false;

    private IEnemy? _existingEnemy = null;

    private string additionalMessage = "";

    private readonly object _drawMapLock = new object();

    // Used to capture the synchronization context of the main thread.
    private SynchronizationContext _syncronizationContext =
        SynchronizationContext.Current ??
        new SynchronizationContext();

    internal void Start()
    {
        SynchronizationContext.SetSynchronizationContext(_syncronizationContext);
        view.ClearScreen();
        world.InitWorld(OnWorldTimeChange);
        do
        {
            if (_existingEnemy != null)
            {
                FightExistingEnemy(_existingEnemy);
            }
            DrawWorldWithLock(world.GetWorldSnapShot(), additionalMessage);
            HandleMoveCommand(view.GetCommand());
        } while (!_gameOver && !_goal);
    }

    private void OnWorldTimeChange(Object? source, Timers.ElapsedEventArgs e)
    {
        var worldEnemy = world.FightingEnemy;
        if (worldEnemy == null)
        {
            DrawWorldWithLock(world.GetWorldSnapShot(), additionalMessage);
        } else
        {
            var args = new WorldEventArgs<IEnemy>(e.SignalTime, worldEnemy);
            _syncronizationContext.Post(_ => HandleEnemyFightEvent(args), null);
        }
    }

    private void DrawWorldWithLock(WorldMap map, string msg)
    {
        lock (_drawMapLock)
        {
            view.DrawWorld(world, map, msg);
        }
    }

    private void HandleEnemyFightEvent(WorldEventArgs<IEnemy> e)
    {
        _existingEnemy = e.Data;
        HandleGameState();
    }

    private void FightExistingEnemy(IEnemy enemy)
    {
        fightController.StartFight(world.Player, enemy);
        
        if (!world.IsGameOver(out _gameOver))
        {
            world.RemoveFightingEnemyFromWorld(enemy);
            _existingEnemy = null;
            world.InitWorld(OnWorldTimeChange);
            view.ClearScreen();
        }
    }

    private void HandleMoveCommand(Move move)
    {
        try
        {
            world.MovePlayerToNextPosition(move);
            bool waitForUserInput = true;
            HandleGameState(waitForUserInput);
        }
        catch (InvalidOperationException e) 
        {
            additionalMessage = view.GetWarningMessageText(world.Player, e.Message);
        }
    }

    private void HandleGameState(bool waitForUserInput = false)
    {
        if (world.IsGameOver(out _gameOver))
        {
            var gameOverMsg = view.GetGameOverText();
            DrawWorldWithLock(world.GetWorldSnapShot(), gameOverMsg);
        }

        // TODO When goal use next world if exist do not end game.
        if (world.IsGoal(out _goal))
        {
            var isGoalMsg = view.GetIsGoalText();
            DrawWorldWithLock(world.GetWorldSnapShot(), isGoalMsg);
        }

        _existingEnemy = world.FightingEnemy;
        if (_existingEnemy != null)
        {
            SetupFightInfoState(_existingEnemy, waitForUserInput);
        }
    }

    private void SetupFightInfoState(IEnemy enemy, bool waitForUserInput)
    {
        world.CloseWorld();
        view.WriteFightInfo(world, enemy, waitForUserInput);
    }
}
