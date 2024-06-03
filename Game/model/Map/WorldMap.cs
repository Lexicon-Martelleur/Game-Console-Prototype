using Game.Model.Terrain;

namespace Game.Model.Map;

internal class WorldMap
{
    internal Cell[,] Cells { get; private set; }
    internal int Height { get; }
    internal int Width { get; }

    internal WorldMap(
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