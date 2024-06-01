using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.constants;
using Game.model.GameArtifact;
using Game.model.Map;
using Game.model.World;

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
        return GetConsistentWidth(artifact?.Symbol ?? cell.Terrain.Symbol, cellWidth);
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

    public void WriteGameInfo(IGameWorld world)
    {
        PrintHealthInfo(world);
        PrintPlayerPosition(world.Player);
        WriteGoalMessage(world.Flag);
    }

    private void PrintHealthInfo(IGameWorld world)
    {
        Console.WriteLine(GetConsistentWidth(
            $"❤️ {world.Player.Name} health: {world.Player.Health} {world.GetTerrainInfo()}",
            100
        ));
    }

    private void PrintPlayerPosition(Player player)
    {
        Console.WriteLine(GetConsistentWidth(
            $"{player.Symbol} {player.Name} position: [{player.Position.x}, {player.Position.y}]",
            100
        ));
    }

    public void WriteGoalMessage(Flag flag)
    {
        Console.WriteLine(GetConsistentWidth(
            $"ℹ️ Take the flag at [{flag.Position.x}, {flag.Position.y}] to win",
            100
        ));
    }

    public void WriteWarningMessage(Player player, string msg)
    {
        Console.WriteLine(GetConsistentWidth(
            $"⚠  {msg}",
            100
        ));
    }

    public void WriteGameOver()
    {
        Console.WriteLine(GetConsistentWidth(
            "⚠  Game over",
            100
        ));
    }

    public void WriteIsGoal()
    {
        Console.WriteLine(GetConsistentWidth(
            "🎉 Congratulation",
            100
        ));
    }
}

