namespace Game.Model.Repository
{
    public interface IRepositoryFactory
    {
        IWorldLogger CreateWorldLogger();
    }
}