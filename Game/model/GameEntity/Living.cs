using Game.model.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.GameEntity;

internal interface Living
{
    uint Health { get; set; }
}
