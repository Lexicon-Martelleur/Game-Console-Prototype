using Game.model.GameEntity;
using Game.model.Map;

using Timers = System.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.World;

internal interface IGameWorld
{
    internal Player Player { get; }

    internal Flag Flag { get; }

    internal Timers.Timer WorldTimer { get; set; }

    internal void InitWorld(Timers.ElapsedEventHandler onWorldTimeChange);

    internal MapHolder UpdateMap();

    internal void UpdatePlayerPosition(Position position);

    internal bool IsGameOver();

    internal bool IsGoal();

    internal string GetTerrainInfo();
}
