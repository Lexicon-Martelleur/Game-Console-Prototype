
namespace Game.model;

internal class Map
{
    internal Cell[,] Cells { get; private set; }
    internal int Height { get; }
    internal int Width { get; }

    internal Map(
        int height,
        int width,
        IEnumerable<GameArtifact> artifacts 
    ) {
        Height = height;
        Width = width;
        Cells = DrawMap(artifacts);
    }

    internal Cell [,] DrawMap(IEnumerable<GameArtifact> artifacts)
    {
        var cells = new Cell[Height, Width];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Position position = new Position(x, y);
                Terrain terrain = new Grass();

                cells[y, x] = new Cell(
                    position,
                    terrain,
                    GetArtifactForPosition(artifacts, position)
                );
            }

        }
        return cells;
    }

    private GameArtifact? GetArtifactForPosition(IEnumerable<GameArtifact> artifacts, Position position)
    {
        foreach (GameArtifact artifact in artifacts)
        {
            if (artifact.Position == position)
            {
                return artifact;
            }
        }
        return null;
    } 
}