using Game.Model.Base;

namespace Game.Model.GameEntity;

public interface IGameEntity : IDiscoverableArtifact
{
    public uint Id { get; }
}
