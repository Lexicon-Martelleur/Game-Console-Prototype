using Game.model.Map;
using Game.model.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.GameEntity;

internal class Ant(uint id, Position position) : IEnemy
{
    public uint Id => id;

    private uint _health = 100;
    public string Symbol => "🐜";

    public string Name => "Ant";

    public uint Health
    {
        get => _health;
        set => _health = value;
    }

    public Position Position
    {
        get => position;
        private set => position = value;
    }

    IWeapon IEnemy.Weapon => new Sword();

    public void UpdatePosition(Position newPosition)
    {
        Position = newPosition;
    }
}
