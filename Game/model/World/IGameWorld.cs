using Game.model.GameEntity;
using Game.model.Map;

using Timers = System.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.model.Weapon;

namespace Game.model.World;

// TODO! Divide in factory (WorldFactory) and Service (WorldService)
internal interface IGameWorld
{
    internal Player Player { get; }

    internal Flag Flag { get; }

    internal IEnemy? FightingEnemy { get; }

    internal IEnumerable<IGameEntity> GameEntities { get; }

    internal Timers.Timer WorldTimer { get; set; }

    internal void InitWorld(Timers.ElapsedEventHandler onWorldTimeChange);

    internal MapHolder UpdateMap();

    internal void UpdatePlayerPosition(Position position);

    internal void UpdateEntityHealth(ILiving entity, IWeapon weapon);

    internal void RemoveFightingEnemyFromWorld(IEnemy enemy);

    internal bool IsGameOver();

    internal bool IsGoal();

    internal string GetTerrainInfo();

    internal void CloseWorld();
}
