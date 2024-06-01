using Game.constants;
using Game.model.GameEntity;
using Game.model.Map;
using Game.model.Terrain;

namespace Game.model.terrain;

internal class Fire : DangerousTerrain
{
    public string Name => "Fire";

    public ConsoleColor Color => ColorSpectrum.Fire;

    public string Symbol => "🔥";

    public uint ReduceHealth() => 30;
}