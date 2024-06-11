using Game.Constant;
using Game.Model.Constant;
using Game.Model.GameEntity;
using Game.Utility;
using System.Numerics;
using System;
using System.Text;

namespace Game.Application.View;

internal class FightView() : IFightView
{
    private string[,]? _previousDrawnFight;

    private readonly int _height = WorldConstant.HEIGHT;

    private readonly int _width = WorldConstant.WIDTH;

    public void DrawArena(IHero hero, IEnemy enemy)
    {

        var currBackground = Console.BackgroundColor;

        if (_previousDrawnFight == null)
        {
            _previousDrawnFight = new string[_height, _width];
        }

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                string currentSymbol = GetConsistentWidth(
                    GetSymbol(x, y, hero, enemy),
                    ViewConstant.CELL_WIDTH);
                if (_previousDrawnFight[y, x] != currentSymbol)
                {
                    Console.SetCursorPosition(x * ViewConstant.CELL_WIDTH, y);
                    Console.BackgroundColor = ColorSpectrum.Cliff;
                    Console.Write($"{currentSymbol}");
                    _previousDrawnFight[y, x] = currentSymbol;
                }
            }
        }

        Console.BackgroundColor = currBackground;
        Console.SetCursorPosition(0, _height);
        WriteFightInfo(hero, enemy);
    }

    private void WriteFightInfo(IHero hero, IEnemy enemy)
    {
        Console.WriteLine(GetConsistentWidth(
        $"Fight: {hero.Name} {hero.Symbol} vs {enemy.Name} {enemy.Symbol}",
        100
        ));
        Console.WriteLine(GetConsistentWidth(
        $"Health {hero.Name} {hero.Symbol}: {hero.Health}",
        100
        ));
        Console.WriteLine(GetConsistentWidth(
        $"Health {enemy.Name} {enemy.Symbol}: {enemy.Health}",
        100
        ));
    }

    private string GetSymbol(int x, int y, IHero player, IEnemy enemy)
    {
        var playerYPosition = _height / 2;
        var playerXPositionfirst = (_width / 4) * 1;
        var enemyYPosition = _height / 2;
        var enenmyXPositionfirst = (_width / 4) * 3;
        if (x == playerXPositionfirst && y == playerYPosition)
        {
            return player.Symbol;
        }
        else if (x == enenmyXPositionfirst && y == enemyYPosition)
        {
            return enemy.Symbol;
        }
        else
        {
            return ".";
        }

    }

    private string GetConsistentWidth(string content, int width)
    {
        return content.GetConsistentWidth(width);
    }

    public FightCommand ReadFightCommand(IHero player)
    {
        var weapons = new StringBuilder();
        foreach (var weapon in player.Weapons)
        {
            weapons.Append($"[{weapon.Name} {weapon.Symbol}]");
        }
        Console.WriteLine(GetConsistentWidth(
            $"Press space to use all weapon ({weapons}) or move:",
            100
        ));
        ConsoleKey key = Console.ReadKey().Key;
        return GetFightCommand(key);
    }

    private FightCommand GetFightCommand(ConsoleKey key) => key switch
    {
        ConsoleKey.UpArrow => FightCommand.UP,
        ConsoleKey.RightArrow => FightCommand.RIGHT,
        ConsoleKey.DownArrow => FightCommand.DOWN,
        ConsoleKey.LeftArrow => FightCommand.LEFT,
        ConsoleKey.Spacebar => FightCommand.WEAPON,
        _ => FightCommand.NONE
    };

    public void ClearScreen()
    {
        Console.Clear();
        _previousDrawnFight = null;
    }
}