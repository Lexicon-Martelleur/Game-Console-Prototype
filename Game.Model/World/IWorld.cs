using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Weapon;
using Game.Constant;
using Game.Model.Events;
using Game.Model.Base;
using Game.Model.GameToken;

namespace Game.Model.World;

// TODO Rename to IWorldService
public interface IWorld
{
    public IHero Hero { get; }

    public IFlag Flag { get; }

    public IEnemy? FightingEnemy { get; }

    // TODO Use when updating world
    public IEnumerable<IDiscoverableArtifact> WorldItems { get; set; }

    public event EventHandler<WorldEventArgs<IHero>>? GameOverEvent;

    public event EventHandler<WorldEventArgs<IEnemy>>? FightEvent;

    public event EventHandler<WorldEventArgs<IDiscoverableArtifact>>? PickTokenEvent;

    public void InitWorld(WorldEvents worldEvents);

    public WorldMap GetWorldSnapShot();

    /// <summary>
    /// Used to move player a new position.
    /// </summary>
    /// <param name="move">Used to determine the move</param>
    /// <exception cref="InvalidOperationException">
    /// Is raised if the new position is invalid, e.g., outside the map or an
    /// <see cref="Stone"></see> terrain.
    /// </exception>
    public void MovePlayerToNextPosition(Move move);

    public void RemoveEnemyFromWorld(IEnemy enemy);

    public string GetTerrainInfo();

    public void UpdateEntityHealth(ICreature entity, IWeapon weapon);

    public bool IsFightOver(IHero player, IEnemy enemy);

    public bool IsHeroDead();

    public void CloseWorld();
}
