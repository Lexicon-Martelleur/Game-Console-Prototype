using Game.constants;
using Game.model.GameArtifact;
using Game.model.Map;

namespace Game.model.terrain
{
    internal class Fire : ITerrain
    {
        public string Name => "Fire";

        public ConsoleColor Color => ColorSpectrum.Fire;

        public string Symbol => "🔥";

    }
}