using Game.constants;
using Game.model.Map;
using Game.model.World;
using Game.view;

namespace Game.controller;

internal class GameController(IGameView view, IGameWorld game)
{
    private bool _gameOver = false;

    private bool _goal = false;

    internal void Start()
    {
        view.ClearScreen();
        
        do
        {
            view.DrawMap(game.UpdateMap());
            view.WriteGameInfo(game);
            HandleMoveCommand(view.GetCommand());
            // Act
            // view.DrawMap(game.UpdateMap());
            // Enemy Action
            // view.DrawMap();

        } while (!_gameOver && !_goal);
    }

    private void HandleMoveCommand(Move move)
    {
        var prevPosition = game.Player.Position;
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
            game.UpdatePlayerPosition(nextPosition);
            HandleGameState();
        }
        catch (InvalidOperationException e) 
        {
            view.WriteWarningMessage(game.Player, e.Message);
        }
    }

    private void HandleGameState()
    {
        _gameOver = game.IsGameOver();
        if (_gameOver )
        {
            view.DrawMap(game.UpdateMap());
            view.WriteGameInfo(game);
            view.WriteGameOver();
        }

        _goal = game.IsGoal();
        if (_goal)
        {
            view.DrawMap(game.UpdateMap());
            view.WriteGameInfo(game);
            view.WriteIsGoal();
        }
    }
}
