using Game.Model.Base;
using Game.Model.Constant;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Terrain;

namespace Game.Model.World;

public abstract class BaseWorld : IWorld
{
    protected readonly int _width = WorldConstant.WIDTH;

    protected readonly int _height = WorldConstant.HEIGHT;

    public static string GetTerrainDescription()
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

    public abstract WorldMap? Map { get; }

    public abstract IFlag Flag { get; }

    public abstract IEnumerable<IDiscoverableArtifact> WorldItems { get; set; }
    public abstract uint WorldTime { get; set; }

    public abstract string Symbol { get; }

    public abstract string Name { get; }

    public abstract WorldMap CreateWorldSnapShot(IHero hero);

    public IDangerousTerrain? GetDangerousTerrain(Position position)
    {
        if (Map?.Cells == null)
        {
            return null;
        }

        Cell? findCell = null;
        foreach (Cell cell in Map.Cells)
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

    public Position GetNewEnemyPosition(IEnemy enemy)
    {
        var currPosition = enemy.Position;
        Position nextPosition = currPosition;
        var isValidPos = false;
        var neigbours = enemy.GetPossibleNextPositions();
        foreach (var position in neigbours)
        {
            nextPosition = position;
            isValidPos = IsValidEnemyPosition(nextPosition);
            if (isValidPos) { break; }
        }
        return nextPosition;
    }

    public abstract bool IsCliffTerrain(Position position);

    public abstract bool IsFireTerrain(Position position);

    public abstract bool IsWaterTerrain(Position position);

    public abstract bool IsStoneTerrain(Position position);

    public bool IsOutsideMap(Position position)
    {
        return (position.x < 0 ||
            position.x >= _width ||
            position.y < 0 ||
            position.y >= _height);
    }

    public bool IsValidEnemyPosition(Position position)
    {
        return (
            IsValidHeroPosition(position) &&
            GetDangerousTerrain(position) == null
        );
    }

    public bool IsValidHeroPosition(Position position)
    {
        return !(
            IsStoneTerrain(position) ||
            IsOutsideMap(position)
        );
    }
}
