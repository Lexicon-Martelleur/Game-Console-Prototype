using Game.Constant;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.World;

namespace Game.view;

internal interface IWorldView
{
    internal void DrawWorld(IWorld world, WorldMap map, string msg);

    internal Move GetCommand();

    internal void ClearScreen();

    internal string GetGameOverText(IHero hero);

    internal string GetWarningMessageText(IHero hero, string msg);

    internal string GetIsGoalText(IGameEntity flag);

    internal void WriteFightInfo(IWorld world, IEnemy enemy, bool waitForUserInput);
}
