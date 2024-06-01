using Game.constants;
using Game.model.Terrain;

namespace Game.model.terrain
{
    internal class Water : DangerousTerrain
    {
        public string Name => "Water";

        public ConsoleColor Color => ColorSpectrum.Water;

        public string Symbol => "🌊";

        public uint ReduceHealth() => 10;
    }
}