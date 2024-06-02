﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.constants;
using Game.model.GameEntity;
using Game.model.Map;
using Game.model.Weapon;
using Game.model.World;

namespace Game.view;

internal class GameView : IGameView
{
    private int _cellWidth = 3;

    private string[,]? _previousDrawnMap;
     
    public void DrawWorld(IGameWorld world, MapHolder map, string msg)
    {
        if (_previousDrawnMap == null)
        {
            _previousDrawnMap = new string[map.Height, map.Width];
        }

        var currBackground = Console.BackgroundColor;

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                string currentSymbol = GetCellSymbol(map.Cells[y, x]);
                if (_previousDrawnMap[y, x] != currentSymbol)
                {
                    Console.SetCursorPosition(x * _cellWidth, y);
                    Console.BackgroundColor = map.Cells[y, x].Terrain.Color;
                    Console.Write($"{currentSymbol}");
                    _previousDrawnMap[y, x] = currentSymbol;
                }
            }
        }

        Console.BackgroundColor = currBackground;
        Console.SetCursorPosition(0, map.Height);
        Console.WriteLine(GetGameInfoText(world, msg));
    }

    private string GetCellSymbol(Cell cell)
    {
        var artifact = cell.Artifact;
        return GetConsistentWidth(artifact?.Symbol ?? cell.Terrain.Symbol, _cellWidth);
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

    private string GetGameInfoText(IGameWorld world, string msg)
    {
        return $"""
        {GetHealthInfoText(world)}
        {GetPlayerPositionText(world.Player)}
        {GetGoalMessageText(world.Flag)}
        {msg}
        """;
    }

    private string GetHealthInfoText(IGameWorld world)
    {
        return GetConsistentWidth(
            $"❤️ {world.Player.Name} health: {world.Player.Health} {world.GetTerrainInfo()}",
            100
        );
    }

    private string GetPlayerPositionText(Player player)
    {
        return GetConsistentWidth(
            $"{player.Symbol} {player.Name} position: [{player.Position.x}, {player.Position.y}]",
            100
        );
    }

    public string GetGoalMessageText(Flag flag)
    {
        return GetConsistentWidth(
            $"ℹ️ Take the flag {flag.Symbol} at [{flag.Position.x}, {flag.Position.y}] to win",
            100
        );
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
        _previousDrawnMap = null;
    }

    public string GetWarningMessageText(Player player, string msg)
    {
        return GetConsistentWidth(
            $"⚠  {msg}",
            100
        );
    }

    public string GetGameOverText()
    {
        return GetConsistentWidth(
            "⚠  Game over",
            100
        ); 
    }

    public string GetIsGoalText()
    {
        return GetConsistentWidth(
            "🎉 Congratulation",
            100
        );
    }

    public void PrintMatchInfo(IGameWorld world, Enemy enemy)
    {
        ClearScreen();
        var player = world.Player;
        var playerWeapons = new StringBuilder();
        foreach (var weapon in player.Weapons)
        {
            playerWeapons.Append($"[{weapon.Name} {weapon.Symbol}]");
        }
        var matchInfo = $"""
            Fight: {player.Name} {player.Symbol} vs {enemy.Name} {enemy.Symbol}
                * {player.Name} {player.Symbol} health: {player.Health}
                * {player.Name} {player.Symbol} weapon: {playerWeapons}
                * {enemy.Name} {enemy.Symbol} health: {player.Health}
                * {enemy.Name} {enemy.Symbol} weapon: {enemy.Weapon.Name} {enemy.Weapon.Symbol}
            Press enter to start the fight
        """;
        Console.WriteLine(matchInfo);
    }
}

