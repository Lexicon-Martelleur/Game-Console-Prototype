using Game.Model.Terrain;
using Game.Model.Base;

namespace Game.Model.Map;

public class Cell(
    Position position,
    ITerrain terrain,
    IGameArtifact? artificat)
{
    public Position Position { get; } = position;
    public ITerrain Terrain { get; } = terrain;
    public IGameArtifact? Artifact { get; set; } = artificat;
}
