using Game.constants;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.World;

namespace Game.view;

internal interface IGameView
{
    internal void DrawWorld(IWorldService world, WorldMap map, string msg);

    internal Move GetCommand();

    internal void ClearScreen();

    internal string GetGameOverText();

    internal string GetWarningMessageText(Hero player, string msg);

    internal string GetIsGoalText();

    internal void WriteFightInfo(IWorldService world, IEnemy enemy, bool waitForUserInput);
}
