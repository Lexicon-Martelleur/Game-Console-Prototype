using Game.constants;

namespace Game.Model.Terrain;

internal class Water : IDangerousTerrain
{
    public string Name => "Water";

    public ConsoleColor Color => ColorSpectrum.Water;

    public string Symbol => "🌊";

    public uint ReduceHealth() => 10;
}