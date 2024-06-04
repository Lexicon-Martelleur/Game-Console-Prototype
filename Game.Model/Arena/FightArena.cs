namespace Game.Model.Arena;

public class FightArena
{
    public ArenaCell[,] Cells { get; private set; }
    public int Height { get; }
    public int Width { get; }

    public FightArena(
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
