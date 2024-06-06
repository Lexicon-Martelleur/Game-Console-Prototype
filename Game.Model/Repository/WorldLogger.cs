
using Game.Infrastructure;

namespace Game.Model.Repository;

public class WorldLogger(IFileLogger fileLogger) : IWorldLogger
{
    public void Write(string logEntry)
    {
        fileLogger.Write(logEntry);
    }
}
