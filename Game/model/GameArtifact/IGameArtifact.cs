using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.model.Map;

namespace Game.model.GameArtifact;

internal interface IGameArtifact
{
    internal string Symbol { get; }

    internal string Name { get; }

    internal Position Position { get; }
}
