using Game.Model.Base;
using Game.Model.Constant;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Terrain;

namespace Game.Model.World;

// TODO! Create an abstract base class.
public class GrassWorld : IWorld
{
    private WorldMap? _worldMap; 

    private readonly int _width = WorldConstant.WIDTH;
    
    private readonly int _height = WorldConstant.HEIGHT;

    private string _name;
    
    private readonly IFlag _flag;

    private IEnumerable<IDiscoverableArtifact> _worldItems;

    public WorldMap? Map { get => _worldMap; }

    public IFlag Flag { get => _flag; }

    public GrassWorld(
        string name,
        IFlag flag,
        IEnumerable<IDiscoverableArtifact> worldItems)
    {
        _name = name;
        _flag = flag;
        _worldItems = worldItems.Append(Flag);
    }

    public string Symbol => "🌱";

    public string Name => _name;

    public IEnumerable<IDiscoverableArtifact> WorldItems
    {
        get => _worldItems;
        set => _worldItems = value;
    }

    public WorldMap CreateWorldSnapShot(IHero hero)
    {
        _worldMap = new WorldMap(
            _height,
            _width,
            UpdateWorld(_worldItems.Append(hero)));
        return _worldMap;
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

    public string GetTerrainDescription()
    {
        var fire = new Fire();
        var water = new Water();
        var cliff = new Cliff();
        var stone = new Stone();
        return $"({stone.Symbol} = -0," +
            $" {water.Symbol} = -{water.ReduceHealth()}," +
            $" {fire.Symbol} = -{fire.ReduceHealth()}," +
            $" {cliff.Symbol} = -{cliff.ReduceHealth()})";
    }

    public IDangerousTerrain? GetDangerousTerrain(Position position)
    {
        Cell? findCell = null;
        var worldCells = _worldMap?.Cells;

        if (worldCells == null)
        {
            return null;
        }
        
        foreach (Cell cell in worldCells)
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
