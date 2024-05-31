using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.constants;
using Game.model.GameArtifact;
using Game.model.Map;

namespace Game.view;

internal class GameView : IGameView
{
    private string[,]? previousDrawnMap;

    private int cellWidth = 3;

    public void DrawMap(MapHolder map)
    {
        if (previousDrawnMap == null)
        {
            previousDrawnMap = new string[map.Height, map.Width];
        }

        var currBackground = Console.BackgroundColor;

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                string currentSymbol = GetCellSymbol(map.Cells[y, x]);
                if (previousDrawnMap[y, x] != currentSymbol)
                {
                    Console.SetCursorPosition(x * cellWidth, y);
                    Console.BackgroundColor = map.Cells[y, x].Terrain.Color;
                    Console.Write($"{currentSymbol}");
                    previousDrawnMap[y, x] = currentSymbol;
                }
            }
        }

        Console.BackgroundColor = currBackground;
        Console.SetCursorPosition(0, map.Height);
    }

    private string GetCellSymbol(Cell cell)
    {
        var artifact = cell.Artifact;
        return GetConsistentCellWidth(artifact?.Symbol ?? cell.Terrain.Symbol);
    }

    private string GetConsistentCellWidth(string cellContent)
    {
        string consistentCellWidth = "   ";

        if (cellContent.Length > cellWidth)
        {
            consistentCellWidth = cellContent.Substring(0, cellWidth);
        }
        else if (cellContent.Length < cellWidth)
        {
            consistentCellWidth = cellContent.PadRight(cellWidth);
        }

        return consistentCellWidth;
    }

    public Move GetCommand()
    {
        ConsoleKey key = Console.ReadKey().Key;
        return GetMove(key);
    }

    private Move GetMove(ConsoleKey key) => key switch
    {
        ConsoleKey.UpArrow => Move.UP,
        ConsoleKey.RightArrow => Move.RIGHT,
        ConsoleKey.DownArrow => Move.DOWN,
        ConsoleKey.LeftArrow => Move.LEFT,
        _ => Move.NONE
    };

    public void ClearScreen()
    {
        Console.Clear();
    }

    public void WriteGameInfo(Player player)
    {
        Console.WriteLine($"{player.Name} has health {player.Health}");
    }

    public void WriteGameOver()
    {
        Console.WriteLine("Game over");
    }

    public void PrintInvalidUserOperation(string msg)
    {
        Console.WriteLine(msg);
    }
}

