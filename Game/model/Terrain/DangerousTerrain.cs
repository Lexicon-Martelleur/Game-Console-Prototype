using Game.model.terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.Terrain;

internal interface DangerousTerrain : ITerrain
{
    internal uint ReduceHealth();
}
