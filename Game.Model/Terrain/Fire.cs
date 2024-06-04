using Game.Constant;

namespace Game.Model.Terrain;

public class Fire : IDangerousTerrain
{
    public string Name => "Fire";

    public ConsoleColor Color => ColorSpectrum.Fire;

    public string Symbol => "🔥";

    public uint ReduceHealth() => 30;
}