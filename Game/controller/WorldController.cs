﻿using Timers = System.Timers;

using Game.constants;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.World;
using Game.view;
using Game.Events;

namespace Game.Controller;

internal class WorldController(
    IWorldView worldView,
    IWorldService worldService,
    FightController fightController)
{
    private bool _gameOver = false;

    private bool _goal = false;

    private IEnemy? _closeEnemy = null;

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
        worldService.InitWorld(OnWorldTimeChange, OnGoal);
        do
        {
            if (_closeEnemy != null)
            {
                FightExistingEnemy(_closeEnemy);
            }
            DrawWorldWithLock(worldService.GetWorldSnapShot(), _additionalMessage);
            HandleMoveCommand(worldView.GetCommand());
        } while (!_gameOver && !_goal);
    }

    private void OnGoal(Object? source, WorldEventArgs<IGameEntity> e)
    {
        _goal = true;
        var isGoalMsg = worldView.GetIsGoalText();
        DrawWorldWithLock(worldService.GetWorldSnapShot(), isGoalMsg);
    }

    private void OnWorldTimeChange(Object? source, Timers.ElapsedEventArgs e)
    {
        var worldEnemy = worldService.FightingEnemy;
        if (worldEnemy == null)
        {
            DrawWorldWithLock(worldService.GetWorldSnapShot(), _additionalMessage);
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
            worldView.DrawWorld(worldService, map, msg);
        }
    }

    private void HandleEnemyFightEvent(WorldTimeEventArgs<IEnemy> e)
    {
        _closeEnemy = e.Data;
        HandleGameState();
    }

    private void FightExistingEnemy(IEnemy enemy)
    {
        fightController.StartFight(worldService.Hero, enemy);
        if (!worldService.IsGameOver(out _gameOver))
        {
            worldService.RemoveFightingEnemyFromWorld(enemy);
            _closeEnemy = null;
            worldService.InitWorld(OnWorldTimeChange, OnGoal);
            worldView.ClearScreen();
        }
    }

    private void HandleMoveCommand(Move move)
    {
        try
        {
            worldService.MovePlayerToNextPosition(move);
            bool waitForUserInput = true;
            HandleGameState(waitForUserInput);
        }
        catch (InvalidOperationException e) 
        {
            _additionalMessage = worldView.GetWarningMessageText(worldService.Hero, e.Message);
        }
    }

    private void HandleGameState(bool waitForUserInput = false)
    {
        if (worldService.IsGameOver(out _gameOver))
        {
            var gameOverMsg = worldView.GetGameOverText();
            DrawWorldWithLock(worldService.GetWorldSnapShot(), gameOverMsg);
        }

        // worldService.PickupExistingFlag();

        // TODO When goal use next world if exist do not end game.
        //if (worldService.IsGoal(out _goal))
        //{
        //    var isGoalMsg = worldView.GetIsGoalText();
        //    DrawWorldWithLock(worldService.GetWorldSnapShot(), isGoalMsg);
        //}

        _closeEnemy = worldService.FightingEnemy;
        if (_closeEnemy != null)
        {
            SetupFightInfoState(_closeEnemy, waitForUserInput);
        }
    }

    private void SetupFightInfoState(IEnemy enemy, bool waitForUserInput)
    {
        worldService.CloseWorld();
        worldView.WriteFightInfo(worldService, enemy, waitForUserInput);
    }
}
