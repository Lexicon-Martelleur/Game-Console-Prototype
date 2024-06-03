using Game.Model.Base;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Terrain;

namespace Game.Model.Arena;

internal class ArenaCell(
    Position position,
    IGameEntity? entity)
{
    internal Position Position { get; } = position;
    internal IGameEntity? Entity { get; set; } = entity;
}
