using System.Collections.Concurrent;
using System.Timers;

using Game.Model.Base;
using Game.Model.Events;
using Game.Model.Repository;
using Game.Model.GameEntity;
using Game.Utility;


namespace Game.Application.Controller;

// TODO! Add some more descriptive method to interface with args, e.g., move command
// Log must show clearly if it is a User event ,e.g., keyboard or a model events .e.g, new world 
public class WorldControllerLogProxy : IWorldController
{
    private readonly IWorldLogger _worldLogger;
    
    private readonly IWorldController _worldController;

    private readonly BlockingCollection<string> _logQueue = new();

    private readonly string logFile = "log.world.txt";

    private readonly string logDir = "resources";

    private readonly string logPath;

    internal WorldControllerLogProxy(
        IWorldController worldController,
        IWorldLogger worldLogger
    )
    {
        try
        {
            _worldLogger = worldLogger;
            _worldController = worldController;
            logPath = logFile.CreateFileWithTimeStampIfNotExist(logDir);
            Task.Factory.StartNew(ProcessLogQueue, TaskCreationOptions.LongRunning);
        }
        catch (IOException ex)
        {
            Console.WriteLine("Could not setup logging proxy");
            Console.WriteLine(ex.ToString());
            Environment.Exit(0);
        }
    }

    public void DrawWorld(bool pause)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(DrawWorld)} is called");
        _worldController.DrawWorld();
    }

    public void FightExistingEnemy(IEnemy? enemy, Action<IHero, IEnemy> startFight)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(FightExistingEnemy)} is called with {enemy}");
        _worldController.FightExistingEnemy(enemy, startFight);
    }

    public void HandleMoveCommand()
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(HandleMoveCommand)} is called");
        _worldController.HandleMoveCommand();
    }

    public void OnFightStart(object? source, WorldEventArgs<IEnemy> e)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(OnFightStart)} is called with {e.Data}");
        _worldController.OnFightStart(source, e);
    }

    public void OnFightStop(object? source, WorldEventArgs<(bool IsHeroDead, IHero Hero)> e)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(OnFightStop)} is called with {e.Data}");
        _worldController.OnFightStop(source, e);
    }

    public void OnGameOver(object? source, WorldEventArgs<IHero> e)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(OnGameOver)} is called with {e.Data}");
        _worldController.OnGameOver(source, e);
    }

    public void OnGameToken(object? source, WorldEventArgs<IDiscoverableArtifact> e)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(OnGameToken)} is called with {e.Data}");
        _worldController.OnGameToken(source, e);
    }

    public void OnGoal(object? source, WorldEventArgs<IGameEntity> e)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(OnGoal)} is called with {e.Data}");
        _worldController.OnGoal(source, e);
    }

    public void OnWorldTime(object? source, ElapsedEventArgs e)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(OnWorldTime)} is called");
        _worldController.OnWorldTime(source, e);
    }

    public bool IsGameOver()
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(IsGameOver)} is called");
        return _worldController.IsGameOver();
    }

    public void InitWorld()
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(InitWorld)} is called");
        _worldController.InitWorld();
    }

    public bool IsFightingEnemy(out IEnemy? enemy)
    {        
        enemy = _worldController.GetFightingEnemy();
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(IsFightingEnemy)} is called");
        return _worldController.IsFightingEnemy(out enemy);
    }

    public IEnemy? GetFightingEnemy()
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(GetFightingEnemy)} is called");
        return _worldController.GetFightingEnemy();
    }

    public void SetupFightInfoState(IEnemy enemy, bool waitForUserInput)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(SetupFightInfoState)} is called with {enemy}");
        _worldController.SetupFightInfoState(enemy, waitForUserInput);
    }

    private void ProcessLogQueue()
    {
        foreach (var logEntry in _logQueue.GetConsumingEnumerable())
        {
            _worldLogger.Write(logEntry);
        }
    }

    public void Dispose()
    {
        _logQueue.CompleteAdding();
    }
}

