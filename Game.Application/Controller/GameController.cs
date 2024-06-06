using Game.Model.GameEntity;

namespace Game.Controller;

internal class GameController(
    SynchronizationContext syncronizationContext,
    IWorldController worldController
)
{
    // Used to capture the synchronization context of the main thread.
    //private SynchronizationContext _syncronizationContext =
    //    SynchronizationContext.Current ??
    //    new SynchronizationContext();

    internal void Start()
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
