using Game.Model.GameEntity;

namespace Game.Application.Controller;

internal class GameController(
    SynchronizationContext syncronizationContext,
    IWorldController worldController,
    IFightController fightController
) : IGameController
{

    public void Start()
    {
        SynchronizationContext.SetSynchronizationContext(syncronizationContext);
        Console.Clear();
        worldController.InitWorld();
        do
        {
            if (worldController.IsFightingEnemy(out IEnemy? enemy))
            {
                var fightAction = fightController.StartFight;
                worldController.FightExistingEnemy(enemy, fightAction);
            }
            worldController.HandleMoveCommand();
            
        } while (!worldController.IsGameOver());
    }
}
