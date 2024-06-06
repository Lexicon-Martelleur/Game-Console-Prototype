using Timers = System.Timers;

using Game.Model.GameEntity;
using Game.Model.World;
using Game.view;
using Game.Model.Events;
using Game.Model.Base;


namespace Game.Controller;

internal class WorldController(
    SynchronizationContext syncronizationContext,
    IWorldView worldView,
    IWorldService worldService) : IWorldController
{
    private bool _gameOver = false;

    private string _additionalMessage = "";

    private readonly object _drawMapLock = new object();

    private WorldEvents GetWorldEvents()
    {
        return (
            OnWorldTime,
            OnGoal,
            OnNewWorld,
            OnGameOver,
            OnFightStart,
            OnGameToken,
            OnFightStop,
            OnInvalidMove
        );
    }

    public void OnGoal(Object? source, WorldEventArgs<IGameEntity> e)
    {
        var isGoalMsg = worldView.GetIsGoalText(e.Data);
        _additionalMessage = isGoalMsg;
        DrawWorld(true);
    }

    public void OnNewWorld(
        Object? source,
        WorldEventArgs<(IWorld PrevWorld, IWorld NewWorld)> e)
    {
        _additionalMessage = worldView.GetNewWorldText(
            e.Data.PrevWorld,
            e.Data.NewWorld);
        worldService.InitWorld(GetWorldEvents());
        DrawWorld();
    }

    public void OnGameOver(Object? source, WorldEventArgs<IHero> e)
    {
        worldService.CloseWorld();
        _gameOver = true;
        var gameOverMsg = worldView.GetGameOverText(e.Data);
        DrawWorld();
    }

    public void OnFightStart(Object? source, WorldEventArgs<IEnemy> e)
    {
        worldService.CloseWorld();
        bool waitForUserInput = true;
        SetupFightInfoState(e.Data, waitForUserInput);
    }

    public void SetupFightInfoState(IEnemy enemy, bool waitForUserInput)
    {
        worldService.CloseWorld();
        worldView.WriteFightInfo(
                worldService,
                enemy,
                waitForUserInput
        );
    }

    public void OnFightStop(
        Object? source,
        WorldEventArgs<(bool IsHeroDead, IHero Hero)> e)
    {
        worldView.ClearScreen();
        if (e.Data.IsHeroDead)
        {
            worldService.CloseWorld();
            _gameOver = true;
            _additionalMessage = worldView.GetGameOverText(worldService.Hero);
        }
    }

    public void OnGameToken(Object? source, WorldEventArgs<IDiscoverableArtifact> e)
    {
        var pickedUpTokenMsg = worldView.GetPickedUpTokenText(e.Data);
        _additionalMessage = pickedUpTokenMsg;
    }

    public void OnInvalidMove(Object? source, WorldEventArgs<Position> e)
    {
        _additionalMessage = worldView.GetInvalidMoveText(worldService.Hero, e.Data);
    }

    public void OnWorldTime(Object? source, Timers.ElapsedEventArgs e)
    {
        var worldEnemy = worldService.FightingEnemy;
        if (worldEnemy == null)
        {
            DrawWorld();
        }
        else
        {
            syncronizationContext.Send(
                _ => SetupFightInfoState(worldEnemy, false),
                null);
        }
    }

    public void InitWorld()
    {
        worldService.InitWorld(GetWorldEvents());
    }

    public bool IsFightingEnemy(out IEnemy? enemy)
    {
        enemy = worldService.FightingEnemy;
        return enemy != null;
    }

    public IEnemy? GetFightingEnemy()
    {
        return worldService.FightingEnemy;
    }

    public bool IsGameOver()
    {
        return _gameOver;
    }

    public void DrawWorld(bool pause = false)
    {
        lock (_drawMapLock)
        {
            var map = worldService.GetWorldSnapShot();
            if (map == null)
            {
                _gameOver = true;
                worldView.WriteGameCongratulation();
                worldService.CloseWorld();
            } 
            else
            {
                var msg = _additionalMessage;
                worldView.DrawWorld(worldService, map, msg, pause);
            }
        }
    }

    public void FightExistingEnemy(IEnemy? enemy, Action<IHero, IEnemy> startFight)
    {
        if (enemy == null)
        {
            return;
        }
        worldService.CloseWorld();
        startFight(worldService.Hero, enemy);
        worldService.InitWorld(GetWorldEvents());
        worldService.RemoveDeadCreatures(enemy);
    }

    public void HandleMoveCommand()
    {
        DrawWorld();
        var move = worldView.GetCommand();
        worldService.MoveHeroToNextPosition(move);
    }
}
