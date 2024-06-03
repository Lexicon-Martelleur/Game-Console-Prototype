using Game.Model.Map;

namespace Game.Model.GameEntity;

internal class Flag(uint id, Position position) : IGameEntity
{
    public uint Id => id;

    public string Symbol => "🚩";

    public string Name => "GoalFlag";

    public Position Position => position;
}
