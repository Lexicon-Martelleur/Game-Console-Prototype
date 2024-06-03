using Game.Model.Base;

namespace Game.Model.Terrain;

internal interface ITerrain : IGameArtefact
{
    internal ConsoleColor Color { get; }
}
