using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.model.Map;

namespace Game.model.GameArtifact;

internal interface Moveable
{
    internal void UpdatePosition(Position newPosition);
}
