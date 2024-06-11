using Game.Model.GameEntity;
using Game.Model.Weapon;

namespace Game.Model.Fight;

/// <summary>
/// An interface describing fight logic and information.
/// </summary>
public interface IFightService
{
    public void HitOpponent(ICreature opponent, IEnumerable<IWeapon> weapons);

    public void HitOpponent(ICreature opponent, IWeapon weapon);

    public bool IsFightOver(IHero player, IEnemy enemy);
}
