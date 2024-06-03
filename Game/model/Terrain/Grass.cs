using Game.constants;

namespace Game.Model.Terrain;

internal class Grass : ITerrain
{
    public string Name => "Grass";

    public ConsoleColor Color => ColorSpectrum.Gras;

    public string Symbol => "🌿";
}
