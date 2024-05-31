
using Game.constants;

namespace Game.model
{
    internal class Water : Terrain
    {
        public string Name => "Water";

        public ConsoleColor Color => ColorSpectrum.Water;
    }
}