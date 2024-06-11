using Game.Constant;
using Game.Model.Base;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.World;

namespace Game.Application.View;

internal interface IWorldView
{
    internal void DrawWorld(
        IWorldService worldService,
        WorldMap map,
        string msg,
        bool pause);

    internal Move GetCommand();

    internal void ClearScreen();

    internal string GetGameOverText(IHero hero);

    internal string GetInvalidMoveText(IHero hero, Position invalidPos);

    internal string GetIsGoalText(IGameEntity flag);

    internal void WriteFightInfo(
        IWorldService worldService, 
        IEnemy enemy,
        bool waitForUserInput);
    
    internal string GetPickedUpTokenText(IDiscoverableArtifact item);

    internal void WriteGameCongratulation(IWorldService worldService);

    internal string GetNewWorldText(IWorld prevWorld, IWorld newWorld);
}
