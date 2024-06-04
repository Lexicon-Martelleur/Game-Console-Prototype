using Game.Model.Weapon;

namespace Game.Model.GameEntity;

public interface IHero : IGameEntity, IMoveable, ICreature
{
    IEnumerable<IWeapon> Weapons { get; }

    IEnumerable<IFlag> Flags { get; set; }

    IEnumerable<ICollectable<IGameEntity>> Tokens { get; set; }
}
