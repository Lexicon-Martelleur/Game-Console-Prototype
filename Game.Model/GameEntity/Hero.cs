using Game.Model.Base;
using Game.Model.GameToken;
using Game.Model.Weapon;

namespace Game.Model.GameEntity;

public class Hero : IHero
{
    public uint Id => _id;

    private uint _health = 100;

    private IEnumerable<IFlag> _flags = [];

    private IEnumerable<IGameToken> _tokens = [];
    
    private Position _position;

    private readonly Position _initialPosition;

    private readonly uint _id;

    public Hero(uint id, Position position)
    {
        _id = id;
        _position = position;
        _initialPosition = position;
    }

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

    public Position InitialPosition => _initialPosition;

    public Position Position
    {
        get => _position;
        private set => _position = value;
    }

    public void UpdatePosition(Position newPosition, Func<Position, bool> IsValidWorldPosition)
    {
        if (IsValidWorldPosition(newPosition))
        {
            Position = newPosition;
        }
    }
}
