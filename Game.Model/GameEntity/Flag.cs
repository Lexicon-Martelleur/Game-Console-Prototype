using Game.Model.Base;

namespace Game.Model.GameEntity;

public class Flag(uint id, Position position, uint gamePoints) : IFlag
{
    public uint Id => id;

    public string Symbol => "🚩";

    public string Name => "GoalFlag";

    public Position Position => position;

    public uint GamePoints => gamePoints;

    public bool IsCollectible(IHero hero, out IGameEntity entity)
    {
        entity = this;
        var isCollectablePostion = hero.Position == Position;
        return isCollectablePostion;
    }
}
