using System.Collections.Concurrent;
using System.Timers;

using Game.Model.Base;
using Game.Model.Events;
using Game.Model.Repository;
using Game.Model.GameEntity;
using Game.Utility;


namespace Game.Controller;

public class WorldControllerLogProxy : IWorldController
{
    private readonly IWorldLogger _worldLogger;
    
    private readonly IWorldController _gameController;

    private readonly BlockingCollection<string> _logQueue = new();

    private readonly string logFile = "log.world.txt";

    private readonly string logDir = "resources";

    private readonly string logPath;

    internal WorldControllerLogProxy(
        IWorldController gameController,
        IWorldLogger worldLogger
    )
    {
        try
        {
            _worldLogger = worldLogger;
            _gameController = gameController;
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
        _gameController.DrawWorld();
    }

    public void FightExistingEnemy(IEnemy? enemy, Action<IHero, IEnemy> startFight)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(FightExistingEnemy)} is called with {enemy}");
        _gameController.FightExistingEnemy(enemy, startFight);
    }

    public void HandleMoveCommand()
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(HandleMoveCommand)} is called");
        _gameController.HandleMoveCommand();
    }

    public void OnFightStart(object? source, WorldEventArgs<IEnemy> e)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(OnFightStart)} is called with {e.Data}");
        _gameController.OnFightStart(source, e);
    }

    public void OnFightStop(object? source, WorldEventArgs<(bool IsHeroDead, IHero Hero)> e)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(OnFightStop)} is called with {e.Data}");
        _gameController.OnFightStop(source, e);
    }

    public void OnGameOver(object? source, WorldEventArgs<IHero> e)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(OnGameOver)} is called with {e.Data}");
        _gameController.OnGameOver(source, e);
    }

    public void OnGameToken(object? source, WorldEventArgs<IDiscoverableArtifact> e)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(OnGameToken)} is called with {e.Data}");
        _gameController.OnGameToken(source, e);
    }

    public void OnGoal(object? source, WorldEventArgs<IGameEntity> e)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(OnGoal)} is called with {e.Data}");
        _gameController.OnGoal(source, e);
    }

    public void OnWorldTime(object? source, ElapsedEventArgs e)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(OnWorldTime)} is called");
        _gameController.OnWorldTime(source, e);
    }

    public bool IsGameOver()
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(IsGameOver)} is called");
        return _gameController.IsGameOver();
    }

    public void InitWorld()
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(InitWorld)} is called");
        _gameController.InitWorld();
    }

    public bool IsFightingEnemy(out IEnemy? enemy)
    {        
        enemy = _gameController.GetFightingEnemy();
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(IsFightingEnemy)} is called");
        return _gameController.IsFightingEnemy(out enemy);
    }

    public IEnemy? GetFightingEnemy()
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(GetFightingEnemy)} is called");
        return _gameController.GetFightingEnemy();
    }

    public void SetupFightInfoState(IEnemy enemy, bool waitForUserInput)
    {
        _logQueue.Add($"[{DateTime.Now}]: ${nameof(SetupFightInfoState)} is called with {enemy}");
        _gameController.SetupFightInfoState(enemy, waitForUserInput);
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

