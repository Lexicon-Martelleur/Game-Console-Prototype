using Timers = System.Timers;

using Game.constants;
using Game.model.GameEntity;
using Game.model.Map;
using Game.model.World;
using Game.view;
using System;
using System.Diagnostics.Tracing;
using Game.Events;

namespace Game.controller;

internal class GameController(
    IGameView view,
    IGameWorld world,
    FightController fightController)
{
    // Used to capture the synchronization context of the main thread
    private SynchronizationContext _syncContext = SynchronizationContext.Current ?? new SynchronizationContext();

    private bool _gameOver = false;

    private bool _goal = false;

    private IEnemy? _existingEnemy = null;

    private string additionalMessage = "";

    private readonly object _drawMapLock = new object();

    internal void Start()
    {
        SynchronizationContext.SetSynchronizationContext(_syncContext);
        view.ClearScreen();
        world.InitWorld(OnWorldTimeChange);
        do
        {
            if (_existingEnemy != null)
            {
                FightExistingEnemy(_existingEnemy);
            }
            DrawWorldWithLock(world.UpdateMap(), additionalMessage);
            HandleMoveCommand(view.GetCommand());
        } while (!_gameOver && !_goal);
    }

    private void OnWorldTimeChange(Object? source, Timers.ElapsedEventArgs e)
    {
        var worldEnemy = world.FightingEnemy;
        if (worldEnemy == null)
        {
            DrawWorldWithLock(world.UpdateMap(), additionalMessage);
        } else
        {
            var args = new WorldEventArgs<IEnemy>(e.SignalTime, worldEnemy);
            _syncContext.Post(_ => HandleEnemyFightEvent(args), null);
        }
    }

    private void DrawWorldWithLock(MapHolder map, string msg)
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
        world.RemoveFightingEnemyFromWorld(enemy);
        _existingEnemy = null;
        world.InitWorld(OnWorldTimeChange);
        view.ClearScreen();
    }

    private void HandleMoveCommand(Move move)
    {
        var prevPosition = world.Player.Position;
        int nextY = prevPosition.y;
        int nextX = prevPosition.x;
        switch (move)
        {
            case Move.UP: nextY--; break;
            case Move.RIGHT: nextX++; break;
            case Move.DOWN: nextY++; break;
            case Move.LEFT: nextX--; break;
            default: break;
        }
        var nextPosition = new Position(nextX, nextY);
        try
        {
            world.UpdatePlayerPosition(nextPosition);
            bool waitForUserInput = true;
            HandleGameState(waitForUserInput);
        }
        catch (InvalidOperationException e) 
        {
            additionalMessage = view.GetWarningMessageText(world.Player, e.Message);
        }
    }

    // Use out key word here in implementation and caller.
    private void HandleGameState(bool waitForUserInput = false)
    {
        _gameOver = world.IsGameOver();
        if (_gameOver )
        {
            var gameOverMsg = view.GetGameOverText();
            DrawWorldWithLock(world.UpdateMap(), gameOverMsg);
        }

        // TODO When goal use next world if exist do not end game.
        _goal = world.IsGoal();
        if (_goal)
        {
            var isGoalMsg = view.GetIsGoalText();
            DrawWorldWithLock(world.UpdateMap(), isGoalMsg);
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
