using Game.Model.Map;
using Game.Model.Weapon;

namespace Game.Model.GameEntity;

public class Chameleon : IEnemy, IDisguisable
{
    private uint _id;

    private Position _position;

    private IEnemy? _disguise = null;

    private uint _health = 100;

    private string _symbol = "🦎";

    public uint Id => _id;

    public string Name => "Chameleon";

    public string Symbol
    {
        get => Disguise == null ? _symbol : Disguise.Symbol;
    }

    public uint Health
    {
        get => _health;
        set => _health = value;
    }

    public Position Position
    {
        get => _position;
        private set => _position = value;
    }

    public IWeapon Weapon {
        get => Disguise == null ? new Sword() : Disguise.Weapon;
    }

    public IEnemy? Disguise { 
        get => _disguise;
        set => _disguise = value;
    }

    Chameleon(uint id, Position position)
    {
        _id = id;
        Position = position;
    }

    public void UpdatePosition(Position newPosition)
    {
        Position = newPosition;
    }
}

