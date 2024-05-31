using Game.constants;
using Game.model;
using Game.view;

namespace Game.controller;

internal class GameController(IGameView view, IGame game)
{
    internal void Start()
    {
        view.ClearScreen();
        bool gameOver = false;
        do
        {
            view.DrawMap(game.UpdateMap());
            HandleMoveCommand(view.GetCommand());
            // Act
            // view.DrawMap(game.UpdateMap());
            // Enemy Action
            // view.DrawMap();

        } while (!gameOver);
        view.ClearScreen();
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
        game.Player.UpdatePosition(nextPosition);
    }
}
