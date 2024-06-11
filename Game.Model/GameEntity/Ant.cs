using Game.Model.Base;
using Game.Model.Weapon;
using Game.Utility;

namespace Game.Model.GameEntity;

public class Ant(uint id, Position position) : IEnemy
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

    public IEnumerable<Position> GetPossibleNextPositions()
    {
        List<Position> neigbours = [
            new Position(position.x, position.y - 1),
            new Position(position.x + 1, position.y - 1),
            new Position(position.x + 1, position.y),
            new Position(position.x + 1, position.y + 1),
            new Position(position.x, position.y + 1),
            new Position(position.x - 1, position.y + 1),
            new Position(position.x - 1, position.y),
            new Position(position.x - 1, position.y - 1),
        ];
        neigbours.Shuffle();
        return neigbours;
    }
}
