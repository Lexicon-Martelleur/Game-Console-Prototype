using Game.Model.Base;
using Game.Model.Terrain;

namespace Game.Model.Map;

public class WorldMap : IGameArtifact
{
    public Cell[,] Cells { get; private set; }
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