
using Game.Model.GameEntity;
using Game.Model.Weapon;
using Game.Model.World;
using Game.view;

namespace Game.Controller;

internal class FightController(IFightView view, IWorldService worldService)
{
    internal void StartFight(IHero player, IEnemy enemy)
    {
        view.ClearScreen();
        do
        {
            view.DrawFight(player, enemy);
            string playerWeaponString = view.ReadWeapon(player);
            foreach (var weapon in player.Weapons)
            {
                worldService.UpdateEntityHealth(enemy, weapon);
            }
            worldService.UpdateEntityHealth(player, enemy.Weapon);
            
        } while (!worldService.IsFightOver(player, enemy));
        
        if (!worldService.IsHeroDead())
        {
            worldService.RemoveEnemyFromWorld(enemy);
        }
    }

    private uint HitOpponent(IWeapon weapon)
    {
        return weapon.ReduceHealth;
    }
}