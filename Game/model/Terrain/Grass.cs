using Game.constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.terrain
{
    internal class Grass : ITerrain
    {
        public string Name => "Grass";

        public ConsoleColor Color => ColorSpectrum.Gras;

        public string Symbol => "🌿";
    }
}
