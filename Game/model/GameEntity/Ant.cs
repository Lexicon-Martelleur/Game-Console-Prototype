using Game.Model.Map;
using Game.Model.Weapon;

namespace Game.Model.GameEntity;

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
