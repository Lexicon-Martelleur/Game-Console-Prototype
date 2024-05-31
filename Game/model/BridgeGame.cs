using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model;

internal class BridgeGame(Player player) : IGame
{
    private readonly int _height = 30;
    private readonly int _width = 50;
    private List<GameArtifact> _artifacts = [
        player
    ];
    
    public Player Player { get => player; }

    public Map UpdateMap()
    {
        return new Map(_height, _width, DrawMap(GetGameArtifacts()));
    }

    internal IEnumerable<GameArtifact> GetGameArtifacts()
    {
        return _artifacts;
    }

    internal Cell[,] DrawMap(IEnumerable<GameArtifact> artifacts)
    {
        var cells = new Cell[_height, _width];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Position position = new Position(x, y);
                Terrain terrain = GetPositionTerrain(position);

                cells[y, x] = new Cell(
                    position,
                    terrain,
                    GetArtifactForPosition(artifacts, position)
                );
            }

        }
        return cells;
    }

    private Terrain GetPositionTerrain(Position position)
    {
        if ((position.x == 10 || position.x == 11) && position.y != 5)
        {
            return new Cliff();
        }
        if ((position.x == 20 || position.x == 21) && position.y != 23)
        {
            return new Fire();
        }
        if ((position.x == 30 || position.x == 31) && position.y != 12)
        {
            return new Water();
        }
        if ((position.x == 40 || position.x == 41) && position.y != 2)
        {
            return new Stone();
        }
        else
        {
            return new Grass();
        }
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
