using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.model.Base;
using Game.model.Map;

namespace Game.model.GameEntity;

internal class Player(uint id, Position position) : IGameEntity, Moveable, Living
{
    public uint Id => id;

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
