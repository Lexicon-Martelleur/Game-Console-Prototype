using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.model.Map;

namespace Game.model.GameEntity;

internal interface IMoveable
{
    internal void UpdatePosition(Position newPosition);
}
