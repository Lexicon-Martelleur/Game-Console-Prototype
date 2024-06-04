
using Game.Model.GameEntity;
using Game.Model.Weapon;
using Game.Model.World;
using Game.view;

namespace Game.Controller;

internal class FightController(IFightView view, IWorld world)
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
                world.UpdateEntityHealth(enemy, weapon);
            }
            world.UpdateEntityHealth(player, enemy.Weapon);
            
        } while (!world.IsFightOver(player, enemy));
        
        if (!world.IsHeroDead())
        {
            world.RemoveEnemyFromWorld(enemy);
        }
    }

    private uint HitOpponent(IWeapon weapon)
    {
        return weapon.ReduceHealth;
    }
}