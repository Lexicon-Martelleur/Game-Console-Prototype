using Game.Model.Constant;
using Game.Model.Repository;
using Game.Model.World;
using Game.Application.View;

namespace Game.Application.Controller;

internal class ControllerFactory
{
    private readonly SynchronizationContext _synchronizationContext;
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly IWorldService _worldService;

    public ControllerFactory(
        SynchronizationContext synchronizationContext,
        IRepositoryFactory repositoryFactory,
        IWorldFactory worldFactory
)
    {
        _synchronizationContext = synchronizationContext;
        _repositoryFactory = repositoryFactory;
        _worldService = worldFactory.CreateWorldService();
    }  

    internal IFightController CreateFightController()
    {
        IFightView fightView = new FightView(
            WorldConstant.WIDTH,
            WorldConstant.HEIGHT);
        return new FightController(fightView, _worldService);
    }

    internal IWorldController CreateWorldController()
    {
        IWorldView worldView = new WorldView();

        IWorldController worldController = new WorldController(
            _synchronizationContext,
            worldView,
            _worldService);

        return new WorldControllerLogProxy(
            worldController,
            _repositoryFactory.CreateWorldLogger()
        );

    }

    internal IGameController CreateGameController(
        IWorldController worldController,
        IFightController fightController
    )
    {
        return new GameController(
            _synchronizationContext,
            worldController,
            fightController);
    }

    // TODO! Use this when Disposable is implemented
    internal IGameController CreateDefaultGameController()
    {
        IFightController fightController = CreateFightController();
        IWorldController worldController = CreateWorldController();
        return new GameController(
            _synchronizationContext,
            worldController,
            fightController);
    }
}
