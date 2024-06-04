using Game.Events;


namespace Game.Model.GameEntity;

internal interface ICollectable<Entity> : IGameEntity
{
    internal event EventHandler<WorldEventArgs<Entity>>? Collected;
    internal bool PickUpExistingEntity(IHero hero, out Entity entity);
}
