using Game.Model.Base;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Terrain;

namespace Game.Model.World;

public interface IWorld
{
    public IFlag Flag { get; }
    public IEnumerable<IDiscoverableArtifact> WorldItems { get; set; }
    WorldMap CreateWorldSnapShot(IHero hero);
    bool IsCliffTerrain(Position position);
    bool IsFireTerrain(Position position);
    bool IsOutsideMap(Position position);
    bool IsStoneTerrain(Position position);
    bool IsWaterTerrain(Position position);
    public IDangerousTerrain? GetDangerousTerrain(Position position);
    public string GetTerrainDescription();
}