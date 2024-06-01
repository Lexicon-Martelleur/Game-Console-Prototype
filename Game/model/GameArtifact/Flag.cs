using Game.model.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.GameArtifact;

internal class Flag(Position position) : IGameArtifact
{
    public string Symbol => "🚩";

    public string Name => "GoalFlag";

    public Position Position => position;
}
