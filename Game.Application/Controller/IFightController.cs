using Game.Model.GameEntity;

namespace Game.Application.Controller;

internal interface IFightController
{
    void StartFight(IHero player, IEnemy enemy);
}