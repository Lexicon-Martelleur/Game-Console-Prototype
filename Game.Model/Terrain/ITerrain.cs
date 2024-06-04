using Game.Model.Base;

namespace Game.Model.Terrain;

public interface ITerrain : IGameArtefact
{
    public ConsoleColor Color { get; }
}
