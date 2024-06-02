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
    // Capture the synchronization context of the main thread
    private SynchronizationContext _syncContext = SynchronizationContext.Current ?? new SynchronizationContext();

    private bool _gameOver = false;

    private bool _goal = false;

    private Enemy? _existingEnemy = null;

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
                world.RemoveFightingEnemyFromWorld(_existingEnemy);
                _existingEnemy = null;
                world.InitWorld(OnWorldTimeChange);
                view.ClearScreen();
                DrawWorldWithLock(world.UpdateMap(), additionalMessage);
                HandleMoveCommand(view.GetCommand());
                
            }
            else { 
               DrawWorldWithLock(world.UpdateMap(), additionalMessage);
               HandleMoveCommand(view.GetCommand());
            }

        } while (!_gameOver && !_goal);
    }

    private void OnWorldTimeChange(Object? source, Timers.ElapsedEventArgs e)
    {
        var worldEnemy = world.FightingEnemy;
        if (worldEnemy != null)
        {
            var args = new WorldEventArgs<Enemy>(e.SignalTime, worldEnemy);
            _syncContext.Post(_ => HandleEnemyFightEvent(args), null);
        } else
        {
            DrawWorldWithLock(world.UpdateMap(), additionalMessage);
        }
    }

    private void HandleEnemyFightEvent(WorldEventArgs<Enemy> e)
    {
        _existingEnemy = e.Data;
        world.CloseWorld();
        view.PrintMatchInfo(world, e.Data); 
    }

    private void FightExistingEnemy(Enemy enemy)
    {
        fightController.StartFight(world.Player, enemy);   
    }

    private void DrawWorldWithLock(MapHolder map, string msg)
    {
        lock (_drawMapLock)
        {
            view.DrawWorld(world, map, msg);
        }
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
            HandleGameState();
        }
        catch (InvalidOperationException e) 
        {
            additionalMessage = view.GetWarningMessageText(world.Player, e.Message);
        }
    }

    private void HandleGameState()
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
    }
}
