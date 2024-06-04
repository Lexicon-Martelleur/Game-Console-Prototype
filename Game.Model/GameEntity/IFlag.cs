namespace Game.Model.GameEntity;

public interface IFlag: ICollectable<IGameEntity>
{
    public uint GamePoints { get; }
}
