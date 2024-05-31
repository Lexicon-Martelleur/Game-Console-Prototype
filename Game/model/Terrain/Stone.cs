using Game.constants;

namespace Game.model.terrain
{
    internal class Stone : ITerrain
    {
        public string Name => "Stone";

        public ConsoleColor Color => ColorSpectrum.Stone;

        public string Symbol => "🪨";
    }
}