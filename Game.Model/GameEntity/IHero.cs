using Game.Model.GameToken;
using Game.Model.Weapon;

namespace Game.Model.GameEntity;

public interface IHero : IGameEntity, IMoveable, ICreature
{
    IEnumerable<IWeapon> Weapons { get; }

    IEnumerable<IFlag> Flags { get; set; }

    IEnumerable<IGameToken> Tokens { get; set; }
}
