using Game.Model.Map;
using Game.Events;

namespace Game.Model.GameEntity;

internal class Flag(uint id, Position position, uint gamePoints) : IFlag
{
    public event EventHandler<WorldEventArgs<IGameEntity>>? Collected;

    public uint Id => id;

    public string Symbol => "🚩";

    public string Name => "GoalFlag";

    public Position Position => position;

    public uint GamePoints => gamePoints;

    public bool PickUpExistingEntity(IHero hero, out IGameEntity entity)
    {
        entity = this;
        var isCollectablePostion = hero.Position == Position;
        if (isCollectablePostion)
        {
            OnFlagPicked();
        }
        return isCollectablePostion;
    }

    private void OnFlagPicked()
    {
        var e = new WorldEventArgs<IGameEntity>(this);
        Collected?.Invoke(this, e);
    }
}
