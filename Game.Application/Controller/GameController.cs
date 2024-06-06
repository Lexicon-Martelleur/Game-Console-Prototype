using Game.Model.GameEntity;

namespace Game.Controller;

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
                worldController.FightExistingEnemy(
                    enemy,
                    fightController.StartFight);
            }
            worldController.DrawWorld();
            worldController.HandleMoveCommand();
        } while (!worldController.IsGameOver());
    }
}
