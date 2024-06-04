using Game.Model.Terrain;
using Game.Model.Base;

namespace Game.Model.Map;

public class Cell(
    Position position,
    ITerrain terrain,
    IGameArtefact? artificat)
{
    public Position Position { get; } = position;
    public ITerrain Terrain { get; } = terrain;
    public IGameArtefact? Artifact { get; set; } = artificat;
}
