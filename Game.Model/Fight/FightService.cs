using Game.Model.GameEntity;
using Game.Model.Weapon;
using Game.Model.World;

namespace Game.Model.Fight;

/// <summary>
/// A class implementing <see cref="IFightService" /> used for fight logic
/// and fight information.
/// </summary>
/// <param name="worldService"></param>
public class FightService(IWorldService worldService) : IFightService
{
    public void HitOpponent(ICreature opponent, IEnumerable<IWeapon> weapons)
    {
        foreach (var weapon in weapons)
        {
            HitOpponent(opponent, weapon);
        }
    }

    public void HitOpponent(ICreature opponent, IWeapon weapon)
    {
        worldService.UpdateCreatureHealth(opponent, weapon);
    }

    public bool IsFightOver(IHero player, IEnemy enemy)
    {
        return player.Health == 0 || enemy.Health == 0;
    }
}
