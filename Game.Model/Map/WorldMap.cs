using Game.Model.Base;

namespace Game.Model.Map;

/// <summary>
/// A class used as a world map container
/// </summary>
public class WorldMap : IGameArtifact
{
    public Cell[,] Cells { get; }
    public int Height { get; }
    public int Width { get; }

    public string Symbol => "🧭";

    public string Name => "Map";

    public WorldMap(
        int height,
        int width,
        Cell[,] cells
    )
    {
        Height = height;
        Width = width;
        Cells = cells;
    }
}