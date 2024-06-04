using Game.Model.Base;

namespace Game.Model.Terrain;

public interface ITerrain : IGameArtifact
{
    public ConsoleColor Color { get; }
}
