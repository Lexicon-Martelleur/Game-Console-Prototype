using Game.Model.Base;

namespace Game.Model.GameEntity;

public interface IFlag: ICollectable<IGameEntity>, IGameEntity 
{
    public uint GamePoints { get; }
}
