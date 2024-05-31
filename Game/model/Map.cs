
namespace Game.model;

internal class Map
{
    internal Cell[,] Cells { get; }
    internal int Height { get; }
    internal int Width { get; }

    internal Map(
        int height,
        int width,
        IEnumerable<GameArtifact> artifacts 
    ) {
        Cells = new Cell[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Position position = new Position(x, y);
                Terrain terrain = new Grass();

                Cells[y, x] = new Cell(
                    position,
                    terrain,
                    GetArtifactForPosition(artifacts, position)
                );
            }

        }
        Height = height;
        Width = width;
    }

    private GameArtifact? GetArtifactForPosition(IEnumerable<GameArtifact> artifacts, Position position)
    {
        foreach (GameArtifact artifact in artifacts)
        {
            if (artifact.InitialPosition == position)
            {
                return artifact;
            }
        }
        return null;
    } 
}