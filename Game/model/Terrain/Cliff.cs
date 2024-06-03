using Game.constants;

namespace Game.Model.Terrain;

internal class Cliff : IDangerousTerrain
{
    public string Name => "Cliff";

    public ConsoleColor Color => ColorSpectrum.Cliff;

    public string Symbol => "☠️";

    public uint ReduceHealth() => 100;
}
