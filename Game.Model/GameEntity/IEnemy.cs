using Game.Model.Base;
using Game.Model.Weapon;

namespace Game.Model.GameEntity;

public interface IEnemy : IGameEntity, IMoveable, ICreature
{
    IWeapon Weapon { get; }

    IEnumerable<Position> GetPossibleNextPositions();
}
