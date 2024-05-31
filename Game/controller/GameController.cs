using Game.constants;
using Game.model;
using Game.view;

namespace Game.controller;

internal class GameController(GameView view, IGame game)
{
    internal void Start()
    {
        bool gameOver = true;
        do
        {
            view.DrawMap(game.Map);
            HandleMoveCommand(view.GetCommand());
            // Act
            // view.DrawMap();
            // Enemy Action
            // view.DrawMap();

        } while (!gameOver);
    }

    private void HandleMoveCommand(Move move)
    {
        var prevPosition = game.Player.CurrentPosition; 
        switch (move)
        {
            case Move.UP:
                var nextPosition = new Position(prevPosition.y - 1, prevPosition.x);
                game.Player.CurrentPosition = nextPosition;
                break;
            case Move.RIGHT:
                nextPosition = new Position(prevPosition.y, prevPosition.x + 1);
                game.Player.CurrentPosition = nextPosition;
                break;
            case Move.DOWN:
                nextPosition = new Position(prevPosition.y + 1, prevPosition.x);
                game.Player.CurrentPosition = nextPosition;
                break;
            case Move.LEFT:
                nextPosition = new Position(prevPosition.y, prevPosition.x - 1);
                game.Player.CurrentPosition = nextPosition;
                break;
            default:
                break;
        }
    }
}
