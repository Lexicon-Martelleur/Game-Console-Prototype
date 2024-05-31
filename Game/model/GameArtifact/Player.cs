using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.model.Map;

namespace Game.model.GameArtifact;

internal class Player(Position position) : IGameArtifact, Moveable, Living
{
    private uint _health = 100;
    public string Symbol => "🐝";

    public string Name => "Player";

    public uint Health {
        get => _health;
        set => _health = value;
    }

    public Position Position
    {
        get => position;
        private set => position = value;
    }

    public void UpdatePosition(Position newPosition)
    {
        Position = newPosition;
    }
}
