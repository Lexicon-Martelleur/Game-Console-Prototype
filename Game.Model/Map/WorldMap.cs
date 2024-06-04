using Game.Model.Terrain;

namespace Game.Model.Map;

public class WorldMap
{
    public Cell[,] Cells { get; private set; }
    public int Height { get; }
    public int Width { get; }

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