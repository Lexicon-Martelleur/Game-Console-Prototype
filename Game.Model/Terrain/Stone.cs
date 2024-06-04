using Game.Constant;

namespace Game.Model.Terrain;

public class Stone : ITerrain
{
    public string Name => "Stone";

    public ConsoleColor Color => ColorSpectrum.Stone;

    public string Symbol => "🪨";
}