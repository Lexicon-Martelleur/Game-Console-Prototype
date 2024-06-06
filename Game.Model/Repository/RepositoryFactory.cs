
using Game.Infrastructure;
using Game.Constant;

namespace Game.Model.Repository;

public class RepositoryFactory : IRepositoryFactory
{
    public IWorldLogger CreateWorldLogger()
    {
        IFileLogger worldFileLogger = new FileLogger(
            LogConstant.WORLD_LOG,
            LogConstant.ROOT_DIR);
        return new WorldLogger(worldFileLogger);
    }
}
