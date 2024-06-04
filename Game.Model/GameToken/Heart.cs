using Game.Model.Base;
using Game.Model.Events;
using Game.Model.GameEntity;

namespace Game.Model.GameToken;

public class Heart(Position position) : IHeart
{
    public string Symbol => "❤️";

    public string Name => "Heart";

    public uint HealthInjection => 10;

    public Position Position => position;

    public event EventHandler<WorldEventArgs<IDiscoverableArtifact>>? Collected;

    public bool PickUpExistingItem(IHero hero, out IDiscoverableArtifact item)
    {
        item = this;
        var isCollectablePostion = hero.Position == Position;
        if (isCollectablePostion)
        {
            hero.Health = hero.Health + HealthInjection;
            OnHeartPicked();
        }
        return isCollectablePostion;
    }

    private void OnHeartPicked()
    {
        var e = new WorldEventArgs<IDiscoverableArtifact>(this);
        Collected?.Invoke(this, e);
    }
}
