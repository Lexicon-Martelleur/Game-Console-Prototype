using Game.Model.Base;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Terrain;

namespace Game.Model.World;

public class GrassWorld : BaseWorld
{
    private WorldMap? _worldMap; 

    private string _name;
    
    private readonly IFlag _flag;

    private IEnumerable<IDiscoverableArtifact> _worldItems;

    private uint _worldTime = 0;

    public override WorldMap? Map { get => _worldMap; }

    public override IFlag Flag { get => _flag; }


    /// <summary>
    /// Used to init and validate the world.
    /// </summary>
    /// <param name="name">The name of the world.</param>
    /// <param name="flag">A flag to be placed in the world.</param>
    /// <param name="worldItems">World items to be placed in the world.</param>
    /// <exception cref="InvalidWorldState">When invalid world state.</exception>
    public GrassWorld(
        string name,
        IFlag flag,
        IEnumerable<IDiscoverableArtifact> worldItems)
    {
        ValidateWorldItems(flag, worldItems);
        _name = name;
        _flag = flag;
        _worldItems = worldItems.Append(Flag);
    }

    public override uint WorldTime {
        get => _worldTime;
        set => _worldTime = value;
    }

    public override string Symbol => "🌱";

    public override string Name => _name;

    public override IEnumerable<IDiscoverableArtifact> WorldItems
    {
        get => _worldItems;
        set => _worldItems = value;
    }

    /// <summary>
    /// Used to validate the worlds before initiated.
    /// </summary>
    /// <param name="flag">A flag to be placed in the world.</param>
    /// <param name="worldItems">World items to be placed in the world.</param>
    /// <exception cref="InvalidWorldState">When invalid world state.</exception>
    private void ValidateWorldItems(
        IFlag flag,
        IEnumerable<IDiscoverableArtifact> worldItems
    )
    {
        if (!IsValidDiscoverableArtifactPosition(flag.Position))
        {
            throw new InvalidWorldState($"Flag have invalid position {flag.Position}");
        }

        foreach (var enemy in worldItems.OfType<IEnemy>())
        {
            if (!IsValidEnemyPosition(enemy.Position)) {  
                throw new InvalidWorldState($"Enemy have invalid position {enemy.Position}");
            }
        }

        foreach (var artifact in worldItems.OfType<IDiscoverableArtifact>())
        {
            if (!IsValidDiscoverableArtifactPosition(artifact.Position))
            {
                throw new InvalidWorldState($"Discoverable artifact have invalid position {artifact.Position}");
            }
        }
    }

    private bool IsValidDiscoverableArtifactPosition(Position position)
    {
        return (
            IsValidHeroPosition(position) &&
            GetDangerousTerrain(position) == null
        );
    }
    
    /// <summary>
    /// Used to get a snapshot of the world.
    /// </summary>
    /// <param name="hero">The hero in the world</param>
    /// <returns>A <see cref="WorldMap"/> of the current state of the world.</returns>
    /// <exception cref="InvalidWorldState">When invalid world state.</exception>
    public override WorldMap CreateWorldSnapShot(IHero hero)
    {
        ValidateWorldItems(_flag, _worldItems);

        if (!IsValidHeroPosition(hero.Position))
        {
            throw new InvalidWorldState($"Hero have invalid position {hero.Position}");
        }

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

    public override bool IsStoneTerrain(Position position)
    {
        return (position.x == 10 || position.x == 11) && position.y != 5;
    }

    public override bool IsFireTerrain(Position position)
    {
        return (position.x == 20 || position.x == 21) && position.y != 23;
    }

    public override bool IsWaterTerrain(Position position)
    {
        return (position.x == 30 || position.x == 31) && position.y != 12;
    }

    public override bool IsCliffTerrain(Position position)
    {
        return (position.x == 40 || position.x == 41) && position.y != 2;
    }
}
