using Game.Controller;
using Game.Model.Constant;
using Game.Model.World;
using Game.view;

var worldView = new WorldView();

var factory = new WorldFactory();
var world = factory.CreateWorldService();

var fightView = new FightView(
    WorldConstant.WIDTH,
    WorldConstant.HEIGHT);
var fightController = new FightController(fightView, world);

var syncronizationContext =
    SynchronizationContext.Current ??
    new SynchronizationContext();

IWorldController worldController = new WorldController(
    syncronizationContext,
    worldView,
    world,
    fightController);

IWorldController worldControllerProxy = new WorldControllerLogProxy(worldController);

var gameController = new GameController(syncronizationContext, worldControllerProxy);

gameController.Start();

// TODO! Implement disposable.
(worldControllerProxy as WorldControllerLogProxy)?.Dispose();
