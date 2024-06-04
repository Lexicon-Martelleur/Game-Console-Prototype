using Timers = System.Timers;

using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Weapon;
using Game.constants;
using Game.Events;

namespace Game.Model.World;

internal interface IWorldService
{
    internal Hero Hero { get; }

    internal Flag Flag { get; }

    internal IEnemy? FightingEnemy { get; }

    internal IEnumerable<IGameEntity> GameEntities { get; }

    internal Timers.Timer WorldTimer { get; set; }

    internal void InitWorld(
        Timers.ElapsedEventHandler onWorldTimeChange,
        EventHandler<WorldEventArgs<IGameEntity>> onGoal
    );

    internal WorldMap GetWorldSnapShot();

    /// <summary>
    /// Used to move player a new position.
    /// </summary>
    /// <param name="move">Used to determine the move</param>
    /// <exception cref="InvalidOperationException">
    /// Is raised if the new position is invalid, e.g., outside the map or an
    /// <see cref="Stone"></see> terrain.
    /// </exception>
    internal void MovePlayerToNextPosition(Move move);

    internal void UpdateEntityHealth(ICreature entity, IWeapon weapon);

    internal void RemoveFightingEnemyFromWorld(IEnemy enemy);

    internal bool IsGameOver(out bool isGameOver);

    internal bool IsGoal(out bool isGoal);

    internal string GetTerrainInfo();

    internal void CloseWorld();
}
