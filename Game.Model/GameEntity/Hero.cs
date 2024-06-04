using Game.Model.Base;
using Game.Model.GameToken;
using Game.Model.Weapon;

namespace Game.Model.GameEntity;

public class Hero(uint id, Position position) : IHero
{
    public uint Id => id;

    private uint _health = 100;

    private IEnumerable<IFlag> _flags = [];

    private IEnumerable<IGameToken> _tokens = [];

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

    public IEnumerable<IFlag> Flags {
        get => _flags;
        set => _flags = value;
    }

    public IEnumerable<IGameToken> Tokens
    {
        get => _tokens;
        set => _tokens = value;
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
