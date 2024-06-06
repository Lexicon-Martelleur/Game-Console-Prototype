using Game.Model.GameEntity;

namespace Game.Controller;

internal class GameController(
    SynchronizationContext syncronizationContext,
    IWorldController worldController
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
                worldController.FightExistingEnemy(enemy);
            }
            worldController.DrawWorld();
            worldController.HandleMoveCommand();
        } while (!worldController.IsGameOver());
    }
}
