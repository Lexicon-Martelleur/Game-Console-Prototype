using Game.Model.Terrain;
using Game.Model.Base;

namespace Game.Model.Map;

/// <summary>
/// A class used as a container for a <see cref="Map"></see> cell.
/// </summary>
/// <param name="position">The map <see cref="Position"/> of the cell</param>
/// <param name="terrain">The <see cref="ITerrain"/> of the cell</param>
/// <param name="artificat">A artifact that the cell may contain.</param>
public class Cell(
    Position position,
    ITerrain terrain,
    IGameArtifact? artificat)
{
    public Position Position { get; } = position;
    public ITerrain Terrain { get; } = terrain;
    public IGameArtifact? Artifact { get; set; } = artificat;
}
