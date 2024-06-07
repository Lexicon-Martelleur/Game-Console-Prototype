using Game.Model.GameEntity;


namespace Game.Model.Base;

public interface ICollectible<Item> : IDiscoverableArtifact
{
    public bool IsCollectible(IHero hero, out Item collectedItem);
}
