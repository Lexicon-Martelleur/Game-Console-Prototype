
using Game.constants;

namespace Game.model
{
    internal class Stone : Terrain
    {
        public string Name => "Stone";

        public ConsoleColor Color => ColorSpectrum.Stone;
    }
}