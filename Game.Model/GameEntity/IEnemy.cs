using Game.Model.Weapon;

namespace Game.Model.GameEntity
{
    public interface IEnemy : IGameEntity, IMoveable, ICreature
    {
        IWeapon Weapon { get; }
    }

}
