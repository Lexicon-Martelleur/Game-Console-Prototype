using Timers = System.Timers;

using Game.Constant;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.World;
using Game.view;
using Game.Model.Events;
using Game.Model.Base;
using Microsoft.VisualBasic;


namespace Game.Controller;

internal class WorldController(
    SynchronizationContext syncronizationContext,
    IWorldView worldView,
    IWorldService worldService,
    FightController fightController) : IWorldController
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
            OnFightStop
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
        WorldEventArgs<(bool IsHeroDead, Game.Model.GameEntity.IHero Hero)> e)
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

    public void OnWorldTime(Object? source, Timers.ElapsedEventArgs e)
    {
        var worldEnemy = worldService.FightingEnemy;
        if (worldEnemy == null)
        {
            DrawWorld();
        }
        else
        {
            var worldEnemyEventArgs = new WorldTimeEventArgs<IEnemy>(
                e.SignalTime,
                worldEnemy);
            syncronizationContext.Post(
                _ => HandleEnemyFightEvent(worldEnemyEventArgs),
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
            var msg = _additionalMessage;
            if (map == null)
            {
                _gameOver = true;

                worldView.WriteGameCongratulation();
            }
            else
            {
                worldView.DrawWorld(worldService, map, msg, pause);
            }
        }
    }

    private void HandleEnemyFightEvent(WorldTimeEventArgs<IEnemy> e)
    {
        if (worldService.FightingEnemy != null)
        {
            bool waitForUserInput = false;
            SetupFightInfoState(e.Data, waitForUserInput);
        }
    }

    public void FightExistingEnemy(IEnemy? enemy)
    {
        if (enemy == null)
        {
            return;
        }
        worldService.CloseWorld();
        fightController.StartFight(worldService.Hero, enemy);
        worldService.InitWorld(GetWorldEvents());
        worldService.RemoveDeadCreatures(enemy);
    }

    public void HandleMoveCommand()
    {
        var move = worldView.GetCommand();
        try
        {
            worldService.MovePlayerToNextPosition(move);
        }
        catch (InvalidOperationException e)
        {
            _additionalMessage = worldView.GetWarningMessageText(worldService.Hero, e.Message);
        }
    }
}
