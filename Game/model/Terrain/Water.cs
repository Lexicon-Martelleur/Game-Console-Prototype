using Game.constants;

namespace Game.model.terrain
{
    internal class Water : ITerrain
    {
        public string Name => "Water";

        public ConsoleColor Color => ColorSpectrum.Water;

        public string Symbol => "🌊";
    }
}