using System.Text;

using Game.Constant;
using Game.Model.Base;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.World;
using Game.Utility;

namespace Game.Application.View;

internal class WorldView : IWorldView
{
    private string[,]? _previousDrawnMap;

    public void DrawWorld(
        IWorldService worldService,
        WorldMap map,
        string msg,
        bool pause)
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
                    Console.SetCursorPosition(x * ViewConstant.CELL_WIDTH, y);
                    Console.BackgroundColor = map.Cells[y, x].Terrain.Color;
                    Console.Write($"{currentSymbol}");
                    _previousDrawnMap[y, x] = currentSymbol;
                }
            }
        }

        Console.BackgroundColor = currBackground;
        Console.SetCursorPosition(0, map.Height);
        Console.WriteLine(GetGameInfoText(worldService, msg));
        if (pause) 
        {
            Console.In.WaitForEnter();
        }
    }

    private string GetCellSymbol(Cell cell)
    {
        var artifact = cell.Artifact;
        return GetConsistentWidth(artifact?.Symbol ?? cell.Terrain.Symbol, ViewConstant.CELL_WIDTH);
    }

    private string GetConsistentWidth(string content, int width)
    {
        return content.GetConsistentWidth(width);
    }

    private string GetGameInfoText(IWorldService worldService, string msg)
    {
        return $"""
        {GetHealthInfoText(worldService)}
        {GetPlayerPositionText(worldService.Hero)}
        {GetGoalMessageText(worldService)}
        {msg}
        """;
    }

    private string GetHealthInfoText(IWorldService worldService)
    {
        return GetConsistentWidth(
            $"❤️ {worldService.Hero.Name} health: {worldService.Hero.Health} {worldService.GetTerrainDescription()}",
            100
        );
    }

    private string GetPlayerPositionText(IHero player)
    {
        return GetConsistentWidth(
            $"{player.Symbol} {player.Name} position: [{player.Position.x}, {player.Position.y}]" +
            $" (UP={ConsoleKey.UpArrow}; Right={ConsoleKey.RightArrow}; Down={ConsoleKey.DownArrow}; Left={ConsoleKey.LeftArrow})",
            100
        );
    }

    private string GetGoalMessageText(IWorldService worldService)
    {
        return GetConsistentWidth(
            $"{worldService.GetGoalMessage()}",
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

    public string GetInvalidMoveText(IHero hero, Position invalidPos)
    {
        return GetConsistentWidth(
            $"⚠  {hero.Name} {hero.Symbol} can not move to coordinates ({invalidPos.x}, {invalidPos.y})",
            100
        );
    }

    public string GetGameOverText(IHero hero)
    {
        return GetConsistentWidth(
            $"⚠  Game over {hero.Name} {hero.Symbol} your health is {hero.Health}",
            100
        ); 
    }

    public string GetIsGoalText(IGameEntity flag)
    {
        return GetConsistentWidth(
            $"🎉 Congratulation you picked flag at position " +
            $"({flag.Position.x}, {flag.Position.y}), " +
            $"press enter to continue to next world",
            100
        );
    }

    public void WriteFightInfo(
        IWorldService worldService,
        IEnemy enemy,
        bool waitForUserInput)
    {
        ClearScreen();
        var player = worldService.Hero;
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
        if (waitForUserInput) 
        {
            Console.In.WaitForEnter();
        }
    }

    public string GetPickedUpTokenText(IDiscoverableArtifact item)
    {
        return GetConsistentWidth(
            $"ℹ️ You picked up {item.Name} {item.Symbol} at position ({item.Position.x}, {item.Position.y})",
            100
        );
    }

    public void WriteGameCongratulation()
    {
        Console.WriteLine("🎉 Congratulation you finished the game 🎉");
        Console.In.WaitForEnter();
    }

    public string GetNewWorldText(IWorld prevWorld, IWorld newWorld)
    {
        return GetConsistentWidth(
            $"ℹ️ World {prevWorld.Name} {prevWorld.Symbol} is conquered, new world {newWorld.Name} {newWorld.Symbol} is your new challenge)",
            100
        );
    }
}
