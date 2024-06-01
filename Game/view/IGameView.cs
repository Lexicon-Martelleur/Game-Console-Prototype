using Game.constants;
using Game.model.GameArtifact;
using Game.model.Map;
using Game.model.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.view;

internal interface IGameView
{
    internal void DrawMap(MapHolder map);

    internal Move GetCommand();

    internal void ClearScreen();

    internal void WriteGameInfo(IGameWorld game);

    internal void WriteGameOver();

    internal void WriteWarningMessage(Player player, string msg);

    internal void WriteGoalMessage(Flag flag);

    internal void WriteIsGoal();
}
