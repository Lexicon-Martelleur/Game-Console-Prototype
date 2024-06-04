using Game.Model.Base;
using Game.Model.GameEntity;
using Game.Model.Map;

namespace Game.Model.World;

// TODO Rename to IWorld
// TODO Move Hero, Flag and GameEntities to this type
public interface IWorldBuilder
{
    WorldMap CreateWorldSnapShot(IEnumerable<IDiscoverableArtifact> worldItems);
    bool IsCliffTerrain(Position position);
    bool IsFireTerrain(Position position);
    bool IsOutsideMap(Position position);
    bool IsStoneTerrain(Position position);
    bool IsWaterTerrain(Position position);
}