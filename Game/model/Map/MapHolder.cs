using Game.model.terrain;
using Game.model.Terrain;

namespace Game.model.Map;

internal class MapHolder
{
    internal Cell[,] Cells { get; private set; }
    internal int Height { get; }
    internal int Width { get; }

    internal MapHolder(
        int height,
        int width,
        Cell[,] cells
    )
    {
        Height = height;
        Width = width;
        Cells = cells;
    }

    internal IDangerousTerrain? GetDangerousTerrain(Position position)
    {
        Cell? findCell = null;
        foreach (Cell cell in Cells)
        {
            if (cell.Position == position) 
            {
                findCell = cell; 
                break;
            }
        }
        var terrain = findCell?.Terrain;
        return terrain as IDangerousTerrain;
    }
}