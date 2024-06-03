using Game.constants;
using Game.Model.GameEntity;
using Game.Model.Map;
using System.Text;

namespace Game.view;

internal class FightView(int height, int width)
{
    private string[,]? _previousDrawnFight;

    private int _cellWidth = 3;

    internal void DrawFight(
        Hero player,
        IEnemy enemy)
    {

        var currBackground = Console.BackgroundColor;

        if (_previousDrawnFight == null)
        {
            _previousDrawnFight = new string[height, width];
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                string currentSymbol = GetConsistentWidth(
                    GetSymbol(x, y, player, enemy),
                    _cellWidth);
                if (_previousDrawnFight[y, x] != currentSymbol)
                {
                    Console.SetCursorPosition(x * _cellWidth, y);
                    Console.BackgroundColor = ColorSpectrum.Cliff;
                    Console.Write($"{currentSymbol}");
                    _previousDrawnFight[y, x] = currentSymbol;
                }
            }
        }

        Console.BackgroundColor = currBackground;
        Console.SetCursorPosition(0, height);
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

    private string GetSymbol(int x, int y, Hero player, IEnemy enemy)
    {
        var playerYPosition = height / 2;
        var playerXPositionfirst = (width / 4) * 1;
        var enemyYPosition = height / 2;
        var enenmyXPositionfirst = (width / 4) * 3;
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
        string consistentCellWidth = "   ";

        if (content.Length > width)
        {
            consistentCellWidth = content.Substring(0, width);
        }
        else if (content.Length < width)
        {
            consistentCellWidth = content.PadRight(width);
        }

        return consistentCellWidth;
    }

    internal string ReadWeapon(Hero player)
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