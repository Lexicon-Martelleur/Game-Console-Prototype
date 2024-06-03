using Game.Model.Base;
using Game.Model.Map;

namespace Game.Model.GameEntity;

internal interface IGameEntity : IGameArtefact
{
    internal uint Id { get; }

    internal Position Position { get; }
}
