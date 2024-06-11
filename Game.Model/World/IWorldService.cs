using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Weapon;
using Game.Constant;
using Game.Model.Events;
using Game.Model.Base;

namespace Game.Model.World;

/// <summary>
/// An interface describing world logic and information.
/// </summary>
public interface IWorldService
{
    public IHero Hero { get; }

    public IWorld? CurrentWorld { get; }

    public IEnemy? FightingEnemy { get; }

    public event EventHandler<WorldEventArgs<IHero>>? GameOverEvent;

    public event EventHandler<WorldEventArgs<IEnemy>>? FightStartEvent;

    public event EventHandler<WorldEventArgs<(bool IsHeroDead, IHero Hero)>>? FightStopEvent;

    public event EventHandler<WorldEventArgs<IDiscoverableArtifact>>? CollectTokenEvent;

    public event EventHandler<WorldEventArgs<IGameEntity>>? PickFlagEvent;

    public event EventHandler<WorldEventArgs<Position>>? InvalidMoveEvent;

    public WorldMap? GetWorldSnapShot();

    public void InitWorld(WorldEvents worldEvents);

    public uint GetTotalTime();

    public string GetGoalMessage();

    public uint GetHeroHealth();

    public void MoveHeroToNextPosition(Move move);

    public void RemoveDeadCreatures(IEnemy enemy);

    public string GetTerrainDescription();

    public void UpdateCreatureHealth(ICreature entity, IWeapon weapon);

    public void CloseWorld();
}
