using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.model.Base;
using Game.model.Map;
using Game.model.Weapon;

namespace Game.model.GameEntity;

internal class Player(uint id, Position position) : Hero
{
    public uint Id => id;

    private uint _health = 100;
    public string Symbol => "🐝";

    public string Name => "Player";

    public uint Health {
        get => _health;
        set => _health = value;
    }

    public IEnumerable<IWeapon> Weapons => [
        new Hammer(),
        new Spear(),
        new Sword(),
        new Arrow()
    ];

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
