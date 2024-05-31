
namespace Game.model;

internal class Map
{
    internal Cell[,] Cells { get; private set; }
    internal int Height { get; }
    internal int Width { get; }

    internal Map(
        int height,
        int width,
        Cell[,] cells 
    ) {
        Height = height;
        Width = width;
        Cells = cells;
    }
}