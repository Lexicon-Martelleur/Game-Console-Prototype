using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.model.GameArtifact;
using Game.model.Map;
using Game.model.terrain;

namespace Game.model.World;

internal class BridgeGameWorld(Player player) : IGameWorld
{
    private readonly int _height = 30;
    private readonly int _width = 50;
    private List<IGameArtifact> _artifacts = [
        player
    ];

    public Player Player { get => player; }

    public MapHolder UpdateMap()
    {
        return new MapHolder(_height, _width, DrawMap(GetGameArtifacts()));
    }

    internal IEnumerable<IGameArtifact> GetGameArtifacts()
    {
        return _artifacts;
    }

    internal Cell[,] DrawMap(IEnumerable<IGameArtifact> artifacts)
    {
        var cells = new Cell[_height, _width];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Position position = new Position(x, y);
                ITerrain terrain = GetPositionTerrain(position);

                cells[y, x] = new Cell(
                    position,
                    terrain,
                    GetArtifactForPosition(artifacts, position)
                );
            }

        }
        return cells;
    }

    private ITerrain GetPositionTerrain(Position position)
    {
        if (IsCliffTerrain(position))
        {
            return new Cliff();
        }
        if (IsFireTerrain(position))
        {
            return new Fire();
        }
        if (IsWaterTerrain(position))
        {
            return new Water();
        }
        if (IsStoneTerrain(position))
        {
            return new Stone();
        }
        else
        {
            return new Grass();
        }
    }

    private bool IsCliffTerrain(Position position)
    {
        return (position.x == 10 || position.x == 11) && position.y != 5;
    }

    private bool IsFireTerrain(Position position)
    {
        return (position.x == 20 || position.x == 21) && position.y != 23;
    }

    private bool IsWaterTerrain(Position position)
    {
        return (position.x == 30 || position.x == 31) && position.y != 12;
    }

    private bool IsStoneTerrain(Position position)
    {
        return (position.x == 40 || position.x == 41) && position.y != 2;
    }

    private IGameArtifact? GetArtifactForPosition(
        IEnumerable<IGameArtifact> artifacts,
        Position position)
    {
        foreach (IGameArtifact artifact in artifacts)
        {
            if (artifact.Position == position)
            {
                return artifact;
            }
        }
        return null;
    }

    public void UpdatePlayerPosition(Position position)
    {
        if (!IsValidPosition(position))
        {
            throw new InvalidOperationException("Player can not move to this position");
        }
        Player.UpdatePosition(position);
        if (IsCliffTerrain(position) ||
            IsFireTerrain(position) ||
            IsWaterTerrain(position)) {
            Player.Health = 0;
        }
    }

    private bool IsValidPosition (Position position)
    {
        if (IsStoneTerrain(position) ||
            position.x < 0 ||
            position.x >= _width ||
            position.y < 0 ||
            position.y >= _height)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsGameOver()
    {
        return Player.Health == 0;
    }
}
