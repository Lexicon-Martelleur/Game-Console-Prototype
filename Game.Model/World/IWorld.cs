using Timers = System.Timers;

using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Weapon;
using Game.Constant;
using Game.Model.Events;

namespace Game.Model.World;

public interface IWorld
{
    public IHero Hero { get; }

    public IFlag Flag { get; }

    public IEnemy? FightingEnemy { get; }

    public IEnumerable<IGameEntity> GameEntities { get; }

    public event EventHandler<WorldEventArgs<IHero>>? GameOver;

    public event EventHandler<WorldEventArgs<IEnemy>>? Fight;

    // public Timers.Timer WorldTimer { get; set; }

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

    public void UpdateEntityHealth(ICreature entity, IWeapon weapon);

    public void RemoveFightingEnemyFromWorld(IEnemy enemy);

    public string GetTerrainInfo();

    public bool IsFightOver(IHero player, IEnemy enemy);

    public bool IsHeroDead();

    public void CloseWorld();
}
