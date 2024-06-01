using Game.constants;
using Game.model.Map;
using Game.model.World;
using Game.view;

namespace Game.controller;

internal class GameController(IGameView view, IGameWorld world)
{
    private bool _gameOver = false;

    private bool _goal = false;

    internal void Start()
    {
        view.ClearScreen();
        
        do
        {
            view.DrawMap(world.UpdateMap());
            view.WriteGameInfo(world);
            HandleMoveCommand(view.GetCommand());
            // Act
            // view.DrawMap(game.UpdateMap());
            // Enemy Action
            // view.DrawMap();

        } while (!_gameOver && !_goal);
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
            view.WriteWarningMessage(world.Player, e.Message);
        }
    }

    private void HandleGameState()
    {
        _gameOver = world.IsGameOver();
        if (_gameOver )
        {
            view.DrawMap(world.UpdateMap());
            view.WriteGameInfo(world);
            view.WriteGameOver();
        }

        // TODO When goal use next world if exist do not end game.
        _goal = world.IsGoal();
        if (_goal)
        {
            view.DrawMap(world.UpdateMap());
            view.WriteGameInfo(world);
            view.WriteIsGoal();
        }
    }
}
