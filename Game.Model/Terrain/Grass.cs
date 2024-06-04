using Game.Constant;

namespace Game.Model.Terrain;

public class Grass : ITerrain
{
    public string Name => "Grass";

    public ConsoleColor Color => ColorSpectrum.Grass;

    public string Symbol => "🌿";
}
