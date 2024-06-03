using Game.Model.Map;
using Game.Model.Weapon;

namespace Game.Model.GameEntity;

internal class Hero(uint id, Position position) : IHero
{
    public uint Id => id;

    private uint _health = 100;

    public string Symbol => "🐝";

    public string Name => "Hero";

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
