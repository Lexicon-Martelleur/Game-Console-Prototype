using Game.Constant;

namespace Game.Model.Terrain;

public class Water : IDangerousTerrain
{
    public string Name => "Water";

    public ConsoleColor Color => ColorSpectrum.Water;

    public string Symbol => "🌊";

    public uint ReduceHealth() => 10;
}