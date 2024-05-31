using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model;

internal class Player(Position position) : GameArtifact, Moveable
{
    public string Symbol => "🐝";

    public string Name => "Player";

    public Position Position {
        get => position;
        private set => position = value;
    }

    public void UpdatePosition(Position newPosition)
    {
        Position = newPosition;
    }
}
