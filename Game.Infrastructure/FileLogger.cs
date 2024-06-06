using Game.Utility;


namespace Game.Infrastructure;

public class FileLogger : IFileLogger
{
    private readonly string _logFile;

    private readonly string _logDir;

    private readonly string _logPath;

    public FileLogger(string logFile, string logDir)
    {
        try
        {
            _logDir = logDir;
            _logFile = logFile;
            _logPath = logFile.CreateFileWithTimeStampIfNotExist(_logDir);
        }
        catch (IOException ex)
        {
            Console.WriteLine("Could not setup logging proxy");
            Console.WriteLine(ex.ToString());
            Environment.Exit(0);
        }
    }

    public void Write(string logEntry)
    {
        using StreamWriter writer = new(
                _logPath,
                true);
        writer.WriteLine(logEntry);
    }

}
