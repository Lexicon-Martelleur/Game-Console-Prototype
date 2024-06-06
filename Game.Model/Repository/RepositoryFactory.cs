
using Game.Infrastructure;

namespace Game.Model.Repository;

public class RepositoryFactory : IRepositoryFactory
{
    public IWorldLogger CreateWorldLogger()
    {
        IFileLogger worldFileLogger = new FileLogger("log.world.txt", "resources");
        return new WorldLogger(worldFileLogger);
    }
}
