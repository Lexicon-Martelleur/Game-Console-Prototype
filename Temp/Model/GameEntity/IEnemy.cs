using Game.Model.Weapon;

namespace Game.Model.GameEntity
{
    internal interface IEnemy : IGameEntity, IMoveable, ICreature
    {
        IWeapon Weapon { get; }
    }

}
