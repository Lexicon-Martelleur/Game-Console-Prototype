using Game.Model.Base;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Terrain;

namespace Game.Model.World;

// TODO Rename to BridgeWorld
// TODO MOve Hero, Flag and GameEntities to this class
public class BridgeWorldBuilder(int _width, int _height) : IWorldBuilder
{   
    public WorldMap CreateWorldSnapShot(IEnumerable<IDiscoverableArtifact> worldItems)
    {
        return new WorldMap(_height, _width, UpdateWorld(worldItems));
    }

    private Cell[,] UpdateWorld(IEnumerable<IDiscoverableArtifact> worldItems)
    {
        var cells = new Cell[_height, _width];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Position position = new Position(x, y);
                IDiscoverableArtifact? gameEntity = GetItemAtPosition(worldItems, position);
                ITerrain terrain = GetTerrainAtPosition(position);
                cells[y, x] = new Cell(position, terrain, gameEntity);
            }
        }
        return cells;
    }

    private IDiscoverableArtifact? GetItemAtPosition(
        IEnumerable<IDiscoverableArtifact> worldItems,
        Position position)
    {
        foreach (IDiscoverableArtifact item in worldItems)
        {
            if (item.Position == position)
            {
                return item;
            }
        }
        return null;
    }

    private ITerrain GetTerrainAtPosition(Position position)
    {
        if (IsCliffTerrain(position))
        {
            return new Cliff();
        }
        else if (IsFireTerrain(position))
        {
            return new Fire();
        }
        else if (IsWaterTerrain(position))
        {
            return new Water();
        }
        else if (IsStoneTerrain(position))
        {
            return new Stone();
        }
        else
        {
            return new Grass();
        }
    }

    public bool IsStoneTerrain(Position position)
    {
        return (position.x == 10 || position.x == 11) && position.y != 5;
    }

    public bool IsFireTerrain(Position position)
    {
        return (position.x == 20 || position.x == 21) && position.y != 23;
    }

    public bool IsWaterTerrain(Position position)
    {
        return (position.x == 30 || position.x == 31) && position.y != 12;
    }

    public bool IsCliffTerrain(Position position)
    {
        return (position.x == 40 || position.x == 41) && position.y != 2;
    }

    public bool IsOutsideMap(Position position)
    {
        return (position.x < 0 ||
            position.x >= _width ||
            position.y < 0 ||
            position.y >= _height);
    }
}
