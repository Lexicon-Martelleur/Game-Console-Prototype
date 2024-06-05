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

var gameController = new WorldController(worldView, world, fightController);

gameController.Start();
