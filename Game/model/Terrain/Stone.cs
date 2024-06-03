using Game.constants;

namespace Game.Model.Terrain;

internal class Stone : ITerrain
{
    public string Name => "Stone";

    public ConsoleColor Color => ColorSpectrum.Stone;

    public string Symbol => "🪨";
}