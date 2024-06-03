using Game.constants;
using Game.model.GameEntity;
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
    internal void DrawWorld(IGameWorld world, MapHolder map, string msg);

    internal Move GetCommand();

    internal void ClearScreen();

    internal string GetGameOverText();

    internal string GetWarningMessageText(Player player, string msg);

    internal string GetIsGoalText();

    internal void WriteFightInfo(IGameWorld world, IEnemy enemy, bool waitForUserInput);
}
