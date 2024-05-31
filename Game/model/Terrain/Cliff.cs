using Game.constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.terrain;

internal class Cliff : ITerrain
{
    public string Name => "Cliff";

    public ConsoleColor Color => ColorSpectrum.Cliff;

    public string Symbol => "☠️";
}
