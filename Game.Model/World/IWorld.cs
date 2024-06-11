using Game.Model.Base;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Terrain;

namespace Game.Model.World;

public interface IWorld : IGameArtifact
{
    WorldMap? Map { get; }
    public IFlag Flag { get; }
    public IEnumerable<IDiscoverableArtifact> WorldItems { get; set; }
    public WorldMap CreateWorldSnapShot(IHero hero);
    public bool IsCliffTerrain(Position position);
    public bool IsFireTerrain(Position position);
    public bool IsOutsideMap(Position position);
    public bool IsValidPlayerPosition(Position position);
    public bool IsStoneTerrain(Position position);
    public bool IsWaterTerrain(Position position);
    public IDangerousTerrain? GetDangerousTerrain(Position position);
    public string GetTerrainDescription();
    public Position GetNewEnemyPosition(IEnemy enemy);
}