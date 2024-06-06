// TODO Rename namespace Game.* to Game.Application.*
using Game.Application.Controller;
using Game.Controller;
using Game.Model.Repository;
using Game.Model.World;

var synchronizationContext =
    SynchronizationContext.Current ??
    new SynchronizationContext();

IRepositoryFactory repositoryFactory = new RepositoryFactory();

IWorldFactory worldFactory = new WorldFactory();

var controllerFactory = new ControllerFactory(
    synchronizationContext,
    repositoryFactory,
    worldFactory);

var fightController = controllerFactory.CreateFightController();

var worldController = controllerFactory.CreateWorldController(fightController);

var gameController = controllerFactory.CreateGameController(worldController);

gameController.Start();

// TODO! Implement disposable.
(worldController as WorldControllerLogProxy)?.Dispose();
