using Game.Model.GameEntity;

namespace Game.Controller
{
    internal interface IFightController
    {
        void StartFight(IHero player, IEnemy enemy);
    }
}