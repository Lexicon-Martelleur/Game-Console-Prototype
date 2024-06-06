using Game.Model.GameEntity;

namespace Game.view;

internal interface IFightView
{
    internal void ClearScreen();

    internal string ReadWeapon(IHero player);

    internal void DrawFight(IHero player, IEnemy enemy);
}
