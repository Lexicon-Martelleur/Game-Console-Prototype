using Game.Model.GameEntity;
using Game.Model.Map;

namespace Game.Model.World
{
    internal interface IWorldBuilder
    {
        WorldMap CreateWorldSnapShot(IEnumerable<IGameEntity> gameEntities);
        bool IsCliffTerrain(Position position);
        bool IsFireTerrain(Position position);
        bool IsOutsideMap(Position position);
        bool IsStoneTerrain(Position position);
        bool IsWaterTerrain(Position position);
    }
}