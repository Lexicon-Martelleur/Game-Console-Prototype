using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model;

internal interface GameArtifact
{
    string Symbol { get; }

    string Name { get; }

    Position Position { get; }
}
