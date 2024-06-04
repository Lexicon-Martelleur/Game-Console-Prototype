namespace Game.Model.GameEntity;

internal interface IFlag: ICollectable<IGameEntity>
{
    internal uint GamePoints { get; }
}
