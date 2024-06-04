using Game.Model.Events;
using Game.Model.GameEntity;


namespace Game.Model.Base;

public interface ICollectable<Item> : IGameArtifact
{
    public event EventHandler<WorldEventArgs<Item>>? Collected;
    public bool PickUpExistingItem(IHero hero, out Item item);
}
