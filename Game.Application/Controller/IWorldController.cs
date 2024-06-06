using Game.Model.Base;
using Game.Model.Events;
using Game.Model.GameEntity;
using Game.Model.World;
using System.Timers;

namespace Game.Application.Controller;

internal interface IWorldController
{
    WorldEvents SubscribedWorldEvents { get; set; }
    void DrawWorld(bool pause = false);
    void FightExistingEnemy(IEnemy? enemy, Action<IHero, IEnemy> startFight);
    void HandleMoveCommand();
    bool IsGameOver();
    void InitWorld();
    bool IsFightingEnemy(out IEnemy? enemy);
    IEnemy? GetFightingEnemy();
    void OnFightStart(object? source, WorldEventArgs<IEnemy> e);
    void OnFightStop(object? source, WorldEventArgs<(bool IsHeroDead, IHero Hero)> e);
    void OnGameOver(object? source, WorldEventArgs<IHero> e);
    void OnGameToken(object? source, WorldEventArgs<IDiscoverableArtifact> e);
    void OnGoal(object? source, WorldEventArgs<IGameEntity> e);
    void OnInvalidMove(object? source, WorldEventArgs<Position> e);
    void OnNewWorld(
        object? source,
        WorldEventArgs<(IWorld PrevWorld, IWorld NewWorld)> e
    );
    void OnWorldTime(object? source, ElapsedEventArgs e);
    void SetupFightInfoState(IEnemy enemy, bool waitForUserInput);
}