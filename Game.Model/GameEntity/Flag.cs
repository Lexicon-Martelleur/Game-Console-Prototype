using Game.Model.Events;
using Game.Model.Base;

namespace Game.Model.GameEntity;

public class Flag(uint id, Position position, uint gamePoints) : IFlag
{
    public event EventHandler<WorldEventArgs<IGameEntity>>? Collected;

    public uint Id => id;

    public string Symbol => "🚩";

    public string Name => "GoalFlag";

    public Position Position => position;

    public uint GamePoints => gamePoints;

    public bool PickUpExistingItem(IHero hero, out IGameEntity entity)
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
