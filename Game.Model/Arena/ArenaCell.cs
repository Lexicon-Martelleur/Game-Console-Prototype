using Game.Model.Base;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Terrain;

namespace Game.Model.Arena;

public class ArenaCell(
    Position position,
    IGameEntity? entity)
{
    public Position Position { get; } = position;
    public IGameEntity? Entity { get; set; } = entity;
}
