
using Game.model.GameEntity;
using Game.model.Weapon;
using Game.model.World;
using Game.view;

namespace Game.controller;

internal class FightController(FightView view, IGameWorld world)
{
    internal void StartFight(Player player, Enemy enemy)
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

    private bool IsFightOver(Player player, Enemy enemy) 
    {
        return player.Health == 0 || enemy.Health == 0;
    }
}