using Game.constants;

namespace Game.model
{
    internal class Fire : Terrain
    {
        public string Name => "Fire";

        public ConsoleColor Color => ColorSpectrum.Fire;
    }
}