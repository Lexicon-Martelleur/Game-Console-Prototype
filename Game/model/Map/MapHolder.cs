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
}