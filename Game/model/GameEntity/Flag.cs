using Game.model.Base;
using Game.model.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.GameEntity;

internal class Flag(uint id, Position position) : IGameEntity
{
    public uint Id => id;

    public string Symbol => "🚩";

    public string Name => "GoalFlag";

    public Position Position => position;
}
