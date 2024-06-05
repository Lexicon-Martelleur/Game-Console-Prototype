using Game.Controller;
using Game.Model.Constant;
using Game.Model.World;
using Game.view;

// World 1 BridgeGameWorld
var worldView = new WorldView();

// TODO Make MOdel public and move to separate project
var factory = new WorldFactory();
var world = factory.CreateWorldService();

// Fight
var fightView = new FightView(
    WorldConstant.WIDTH,
    WorldConstant.HEIGHT);
var fightController = new FightController(fightView, world);

// TODO Controller should have a list of worlds
var gameController = new WorldController(worldView, world, fightController);

gameController.Start();
