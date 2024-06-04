using Game.Model.Terrain;
using Game.Model.Base;

namespace Game.Model.Map;

internal class Cell(
    Position position,
    ITerrain terrain,
    IGameArtefact? artificat)
{
    internal Position Position { get; } = position;
    internal ITerrain Terrain { get; } = terrain;
    internal IGameArtefact? Artifact { get; set; } = artificat;
}
