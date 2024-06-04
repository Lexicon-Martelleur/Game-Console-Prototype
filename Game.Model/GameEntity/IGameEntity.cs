using Game.Model.Base;
using Game.Model.Map;

namespace Game.Model.GameEntity;

public interface IGameEntity : IGameArtefact
{
    public uint Id { get; }

    public Position Position { get; }
}
