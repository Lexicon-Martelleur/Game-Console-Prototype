using System.Collections.Concurrent;
using System.Timers;

using Game.Model.Base;
using Game.Model.Events;
using Game.Model.Repository;
using Game.Model.GameEntity;
using System.Runtime.CompilerServices;
using Game.Model.World;


namespace Game.Application.Controller;

// TODO! Extract base class / bas functionality for logging logic and constants.
// TODO! Add some more descriptive method to interface with args, e.g., move command
// Log must show clearly if it is a User event ,e.g., keyboard or a model events .e.g, new world 
public class WorldControllerLogProxy : IWorldController
{
    private readonly IWorldLogger _worldLogger;
    
    private readonly IWorldController _worldController;

    private readonly BlockingCollection<string> _logQueue = new();

    private readonly string METHOD_CALL = "METHOD_CALL";

    private readonly string MODEL_EVENT = "MODEL_EVENT";

    private readonly string USER_EVENT = "USER_EVENT";

    public WorldEvents SubscribedWorldEvents { get; set; }

    internal WorldControllerLogProxy(
        IWorldController worldController,
        IWorldLogger worldLogger
    )
    {
        try
        {
            _worldLogger = worldLogger;
            _worldController = worldController;
            Task.Factory.StartNew(ProcessLogQueue, TaskCreationOptions.LongRunning);
        }
        catch (IOException ex)
        {
            Console.WriteLine("Could not setup logging proxy");
            Console.WriteLine(ex.ToString());
            Environment.Exit(0);
        }
    }

    private void WrappEvents()
    {
        var worldEvents = _worldController.SubscribedWorldEvents;
        worldEvents.OnWorldTime = OnWorldTime;
        worldEvents.OnGoal = OnGoal;
        worldEvents.OnNewWorld = OnNewWorld;
        worldEvents.OnGameOver = OnGameOver;
        worldEvents.OnFightStart = OnFightStart;
        worldEvents.OnGameToken = OnGameToken;
        worldEvents.OnFightStop = OnFightStop;
        worldEvents.OnInvalidMove = OnInvalidMove;
        SubscribedWorldEvents = worldEvents;
        _worldController.SubscribedWorldEvents = SubscribedWorldEvents;
    }

    public void InitWorld()
    {
        WrappEvents();
        LogMethodCall("Initializing world");
        _worldController.InitWorld();
    }

    private void LogUserEvent(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Log(USER_EVENT, message, memberName, filePath, lineNumber);
    }

    private void LogModelEvent(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Log(MODEL_EVENT, message, memberName, filePath, lineNumber);
    }

    private void LogMethodCall(
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Log(METHOD_CALL, message, memberName, filePath, lineNumber);
    }

    private void Log(
        string eventType,
        string message,
        string memberName = "",
        string filePath = "",
        int lineNumber = 0)
    {
        string logEntry = $"{DateTime.Now} " +
            $"[{eventType}] " +
            $"[{Path.GetFileName(filePath)}:{lineNumber}] " +
            $"{memberName}: {message}";
        _logQueue.Add(logEntry);
    }

    public void DrawWorld(bool pause)
    {
        LogMethodCall($"Called with pause = {pause}");
        _worldController.DrawWorld(pause);
    }

    public void FightExistingEnemy(IEnemy? enemy, Action<IHero, IEnemy> startFight)
    {
        LogMethodCall($"Called with enemy = {enemy}");
        _worldController.FightExistingEnemy(enemy, startFight);
    }

    public void HandleMoveCommand()
    {
        LogUserEvent("Move command issued");
        _worldController.HandleMoveCommand();
    }

    public void OnFightStart(object? source, WorldEventArgs<IEnemy> e)
    {
        LogModelEvent($"Fight started with enemy = {e.Data}");
        _worldController.OnFightStart(source, e);
    }

    public void OnInvalidMove(object? source, WorldEventArgs<Position> e)
    {
        LogModelEvent($"Invalid move = {e.Data}");
        _worldController.OnInvalidMove(source, e);
    }

    public void OnNewWorld(object? source, WorldEventArgs<(IWorld PrevWorld, IWorld NewWorld)> e)
    {
        LogModelEvent($"New world = {e.Data.NewWorld}, previous world = {e.Data.PrevWorld}");
        _worldController.OnNewWorld(source, e);
    }

    public void OnFightStop(object? source, WorldEventArgs<(bool IsHeroDead, IHero Hero)> e)
    {
        LogModelEvent($"Fight stopped with result = {e.Data}");
        _worldController.OnFightStop(source, e);
    }

    public void OnGameOver(object? source, WorldEventArgs<IHero> e)
    {
        LogModelEvent($"Game over for hero = {e.Data}");
        _worldController.OnGameOver(source, e);
    }

    public void OnGameToken(object? source, WorldEventArgs<IDiscoverableArtifact> e)
    {
        LogModelEvent($"Game token discovered = {e.Data}");
        _worldController.OnGameToken(source, e);
    }

    public void OnGoal(object? source, WorldEventArgs<IGameEntity> e)
    {
        LogModelEvent($"Goal reached with entity = {e.Data}");
        _worldController.OnGoal(source, e);
    }

    public void OnWorldTime(object? source, ElapsedEventArgs e)
    {
        LogModelEvent("World time event triggered");
        _worldController.OnWorldTime(source, e);
    }

    public bool IsGameOver()
    {
        LogMethodCall("Checking if game is over");
        return _worldController.IsGameOver();
    }

    public bool IsFightingEnemy(out IEnemy? enemy)
    {
        bool result = _worldController.IsFightingEnemy(out enemy);
        LogMethodCall($"Checking if fighting enemy: {enemy}");
        return result;
    }

    public IEnemy? GetFightingEnemy()
    {
        LogMethodCall("Getting current fighting enemy");
        return _worldController.GetFightingEnemy();
    }

    public void SetupFightInfoState(IEnemy enemy, bool waitForUserInput)
    {
        LogMethodCall($"Setting up fight info state for enemy = {enemy}, wait for user input = {waitForUserInput}");
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

