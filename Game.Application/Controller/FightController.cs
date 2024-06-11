
using Game.Model.GameEntity;
using Game.Application.View;
using Game.Model.Fight;
using Game.Constant;
using System;

namespace Game.Application.Controller;

internal class FightController(IFightView view, IFightService fightService) : IFightController
{
    private readonly object _drawArenaLock = new object();

    public void StartFight(IHero hero, IEnemy enemy)
    {
        view.ClearScreen();
        do
        {
            view.DrawArena(hero, enemy);
            HandleFightCommand(hero, enemy);
            // fightService.HitOpponent(hero, enemy.Weapon);

        } while (!fightService.IsFightOver(hero, enemy));
    }

    public void HandleFightCommand(IHero hero, IEnemy enemy)
    {
        try
        {
            DrawArena();
            FightCommand command = view.ReadFightCommand(hero);
            if (command == FightCommand.WEAPON)
            {
                fightService.HitOpponent(enemy, hero.Weapons);
            }
            else
            {
                // fightService.MoveHeroToNextPosition(move);
            }
        }
        catch (Exception e) { Console.WriteLine(e); }
    }

    public void DrawArena(bool pause = false)
    {
        lock (_drawArenaLock)
        {
            // var map = _fightService.GetWorldSnapShot();
            //if (map == null)
            //{
            //    _gameOver = true;
            //    _worldView.WriteGameCongratulation(_worldService);
            //    _worldService.CloseWorld();
            //}
            //else if (_worldService.GetHeroHealth() == 0)
            //{
            //    _gameOver = true;
            //    var msg = _worldView.GetGameOverText(_worldService.Hero);
            //    _worldView.DrawWorld(_worldService, map, msg, pause);
            //    _worldService.CloseWorld();
            //}
            //else
            //{
            //    var msg = _additionalMessage;
            //    _worldView.DrawWorld(_worldService, map, msg, pause);
            //}
        }
    }
}