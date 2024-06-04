using Game.Model.Base;

namespace Game.Model.GameEntity;

public interface IMoveable
{
    public void UpdatePosition(Position newPosition);
}
