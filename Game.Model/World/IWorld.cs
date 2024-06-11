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

    /// <summary>
    /// Used to get a snapshot of the world.
    /// </summary>
    /// <param name="hero">The hero in the world</param>
    /// <returns>A <see cref="WorldMap"/> of the current state of the world.</returns>
    /// <exception cref="InvalidWorldState">When invalid world state.</exception>
    public WorldMap CreateWorldSnapShot(IHero hero);
    public bool IsCliffTerrain(Position position);
    public bool IsFireTerrain(Position position);
    public bool IsOutsideMap(Position position);
    public bool IsValidHeroPosition(Position position);
    public bool IsValidEnemyPosition(Position position);
    public bool IsStoneTerrain(Position position);
    public bool IsWaterTerrain(Position position);
    public IDangerousTerrain? GetDangerousTerrain(Position position);
    public string GetTerrainDescription();
    public Position GetNewEnemyPosition(IEnemy enemy);
}
