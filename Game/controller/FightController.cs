
using Game.Model.GameEntity;
using Game.Model.Weapon;
using Game.Model.World;
using Game.view;

namespace Game.Controller;

internal class FightController(FightView view, IWorld world)
{
    internal void StartFight(IHero player, IEnemy enemy)
    {
        bool fightIsOver = false;
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
            fightIsOver = IsFightOver(player, enemy);
            
        } while (!fightIsOver);
    }

    private uint HitOpponent(IWeapon weapon)
    {
        return weapon.ReduceHealth;
    }

    private bool IsFightOver(IHero player, IEnemy enemy) 
    {
        return player.Health == 0 || enemy.Health == 0;
    }
}