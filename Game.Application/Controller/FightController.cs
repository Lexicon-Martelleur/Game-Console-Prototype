
using Game.Model.GameEntity;
using Game.Model.Weapon;
using Game.Model.World;
using Game.view;

namespace Game.Controller;

internal class FightController(IFightView view, IWorldService worldService) : IFightController
{
    public void StartFight(IHero player, IEnemy enemy)
    {
        view.ClearScreen();
        do
        {
            view.DrawFight(player, enemy);
            string playerWeaponString = view.ReadWeapon(player);
            foreach (var weapon in player.Weapons)
            {
                worldService.UpdateCreatureHealth(enemy, weapon);
            }
            worldService.UpdateCreatureHealth(player, enemy.Weapon);

        } while (!worldService.IsFightOver(player, enemy));
    }

    private uint HitOpponent(IWeapon weapon)
    {
        return weapon.ReduceHealth;
    }
}