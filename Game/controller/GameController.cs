using Game.constants;
using Game.model.Map;
using Game.model.World;
using Game.view;

namespace Game.controller;

internal class GameController(IGameView view, IGameWorld game)
{
    private bool _gameOver = false;

    internal void Start()
    {
        view.ClearScreen();
        
        do
        {
            view.DrawMap(game.UpdateMap());
            view.WriteGameInfo(game.Player);
            HandleMoveCommand(view.GetCommand());
            // Act
            // view.DrawMap(game.UpdateMap());
            // Enemy Action
            // view.DrawMap();

        } while (!_gameOver);
        view.WriteGameOver();
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
            view.PrintPlayerPosition(game.Player);
        }
        catch (InvalidOperationException e) 
        {
            view.PrintInvalidPlayerPosition(game.Player, e.Message);
        }
    }

    private void HandleGameState()
    {
        _gameOver = game.IsGameOver();
        if (_gameOver )
        {
            view.DrawMap(game.UpdateMap());
            view.WriteGameInfo(game.Player);
        }
    }
}
