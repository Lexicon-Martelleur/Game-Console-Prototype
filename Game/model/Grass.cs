using Game.constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model
{
    internal class Grass : Terrain
    {
        public string Name => "Grass";

        public ConsoleColor Color => ColorSpectrum.Gras;
    }
}
