using Game.Model.Weapon;

namespace Game.Model.GameEntity
{
    internal interface IHero : IGameEntity, IMoveable, ICreature
    {
        IEnumerable<IWeapon> Weapons { get; }
    }
}
