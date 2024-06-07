using Game.Model.Base;

namespace Game.Model.GameEntity;

public interface IFlag: ICollectible<IGameEntity>, IGameEntity 
{
    public uint GamePoints { get; }
}
