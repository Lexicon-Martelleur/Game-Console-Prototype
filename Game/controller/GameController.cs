using Timers = System.Timers;

using Game.constants;
using Game.model.GameEntity;
using Game.model.Map;
using Game.model.World;
using Game.view;

namespace Game.controller;

internal class GameController(IGameView view, IGameWorld world)
{
    private bool _gameOver = false;

    private bool _goal = false;

    private string additionalMessage = "";

    private readonly object _drawMapLock = new object();

    internal void Start()
    {
        view.ClearScreen();
        world.InitWorld(OnWorldTimeChange);
        do
        {
            DrawWorldWithLock(world.UpdateMap(), additionalMessage);
            HandleMoveCommand(view.GetCommand());
            // Act
            // view.DrawMap(game.UpdateMap());
            // Enemy Action
            // view.DrawMap();
        } while (!_gameOver && !_goal);
    }

    private void OnWorldTimeChange(Object? source, Timers.ElapsedEventArgs e)
    {
        // Console.WriteLine("The event was triggered at {0:HH:mm:ss.fff}", e.SignalTime);
        DrawWorldWithLock(world.UpdateMap(), additionalMessage);
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
