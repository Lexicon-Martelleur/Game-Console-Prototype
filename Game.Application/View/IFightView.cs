using Game.Constant;
using Game.Model.GameEntity;

namespace Game.Application.View;

internal interface IFightView
{
    internal void ClearScreen();

    internal FightCommand ReadFightCommand(IHero player);

    internal void DrawArena(IHero player, IEnemy enemy);
}
