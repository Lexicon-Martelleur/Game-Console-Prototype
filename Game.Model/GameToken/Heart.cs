using Game.Model.Base;
using Game.Model.GameEntity;

namespace Game.Model.GameToken;

public class Heart(Position position) : IHeart
{
    public string Symbol => "❤️";

    public string Name => "Heart";

    public uint HealthInjection => 10;

    public Position Position => position;

    public bool IsCollectible(IHero hero, out IDiscoverableArtifact item)
    {
        item = this;
        var isCollectablePostion = hero.Position == Position;
        return isCollectablePostion;
    }
}
