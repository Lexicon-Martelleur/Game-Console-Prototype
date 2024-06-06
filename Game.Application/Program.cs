// TODO! Implement proxy for game controller and fight controller.

// TODO! Update logging proxy.

using Game.Application.Controller;
using Game.Model.Repository;
using Game.Model.World;

var synchronizationContext =
    SynchronizationContext.Current ??
    new SynchronizationContext();

IRepositoryFactory repositoryFactory = new RepositoryFactory();

IWorldFactory grassWorldFactory = new GrassWorldFactory();

var controllerFactory = new ControllerFactory(
    synchronizationContext,
    repositoryFactory,
    grassWorldFactory);

var fightController = controllerFactory.CreateFightController();

var worldController = controllerFactory.CreateWorldController();

var gameController = controllerFactory.CreateGameController(
    worldController,
    fightController
);

gameController.Start();

// TODO! Implement disposable.
(worldController as WorldControllerLogProxy)?.Dispose();
