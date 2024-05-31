using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model;

internal class Cell(
    Position position,
    Terrain terrain,
    GameArtifact? artificat)
{
    public Position Position { get; } = position;
    public Terrain Terrain { get; } = terrain;
    public GameArtifact? Artifact { get; set; } = artificat;
}
