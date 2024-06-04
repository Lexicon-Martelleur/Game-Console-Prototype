using Game.Model.Map;

namespace Game.Model.GameEntity;

public interface IMoveable
{
    public void UpdatePosition(Position newPosition);
}
