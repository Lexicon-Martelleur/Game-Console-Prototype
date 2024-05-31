using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.model.GameArtifact;
using Game.model.terrain;

namespace Game.model.Map;

internal class Cell(
    Position position,
    ITerrain terrain,
    IGameArtifact? artificat)
{
    internal Position Position { get; } = position;
    internal ITerrain Terrain { get; } = terrain;
    internal IGameArtifact? Artifact { get; set; } = artificat;
}
