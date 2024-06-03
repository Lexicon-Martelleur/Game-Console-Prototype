using Game.constants;
using Game.model.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.terrain;

internal class Cliff : IDangerousTerrain
{
    public string Name => "Cliff";

    public ConsoleColor Color => ColorSpectrum.Cliff;

    public string Symbol => "☠️";

    public uint ReduceHealth() => 100;
}
