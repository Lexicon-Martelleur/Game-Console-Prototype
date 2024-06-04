using Game.Model.Map;

namespace Game.Model.GameEntity;

internal interface IMoveable
{
    internal void UpdatePosition(Position newPosition);
}
