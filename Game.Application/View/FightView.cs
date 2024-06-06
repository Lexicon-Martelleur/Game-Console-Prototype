using Game.Constant;
using Game.Model.Constant;
using Game.Model.GameEntity;
using Game.Utility;
using System.Text;

namespace Game.Application.View;

internal class FightView() : IFightView
{
    private string[,]? _previousDrawnFight;

    private readonly int _height = WorldConstant.HEIGHT;

    private readonly int _width = WorldConstant.WIDTH;

    public void DrawFight(
        IHero player,
        IEnemy enemy)
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
                    GetSymbol(x, y, player, enemy),
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
        Console.WriteLine(GetConsistentWidth(
            $"Fight: {player.Name} {player.Symbol} vs {enemy.Name} {enemy.Symbol}",
            100
        ));
        Console.WriteLine(GetConsistentWidth(
            $"Health {player.Name} {player.Symbol}: {player.Health}",
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

    public string ReadWeapon(IHero player)
    {
        var weapons = new StringBuilder();
        foreach (var weapon in player.Weapons)
        {
            weapons.Append($"[{weapon.Name} {weapon.Symbol}]");
        }
        Console.WriteLine(GetConsistentWidth(
            $"Press enter to use all weapon ({weapons}):",
            100
        ));
        return Console.ReadLine() ?? "sword";
    }

    public void ClearScreen()
    {
        Console.Clear();
        _previousDrawnFight = null;
    }
}