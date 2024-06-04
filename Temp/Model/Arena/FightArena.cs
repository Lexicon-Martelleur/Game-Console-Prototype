namespace Game.Model.Arena;

internal class FightArena
{
    internal ArenaCell[,] Cells { get; private set; }
    internal int Height { get; }
    internal int Width { get; }

    internal FightArena(
        int height,
        int width,
        ArenaCell[,] cells
    )
    {
        Height = height;
        Width = width;
        Cells = cells;
    }
}
