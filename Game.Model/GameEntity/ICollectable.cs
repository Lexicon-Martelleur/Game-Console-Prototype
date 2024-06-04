using Game.Model.Events;


namespace Game.Model.GameEntity;

public interface ICollectable<Entity> : IGameEntity
{
    public event EventHandler<WorldEventArgs<Entity>>? Collected;
    public bool PickUpExistingEntity(IHero hero, out Entity entity);
}
