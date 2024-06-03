
using Game.Controller;
using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.World;
using Game.view;

// World 1 BridgeGameWorld
var view = new GameView();
// TODO Implement ID generator
var heroEntity = new Hero(1, new Position(0, 0));
var flagBridgeGameWorld = new Flag(2, new Position(48, 28));
var antOneBridgeGameWorld = new Ant(3, new Position(19, 23));
var antTwoBridgeGameWorld = new Ant(4, new Position(29, 12));
var antThreeBridgeGameWorld = new Ant(5, new Position(39, 2));
var antFourBridgeGameWorld = new Ant(6, new Position(3, 3));
IEnumerable<IGameEntity> entitiesBridgeGameWorld = [
    antOneBridgeGameWorld,
    antTwoBridgeGameWorld,
    antThreeBridgeGameWorld,
    antFourBridgeGameWorld
];

var bridgeWorldBuilder = new BridgeWorldBuilder();

var world = new WorldService(
    heroEntity,
    flagBridgeGameWorld, 
    entitiesBridgeGameWorld,
    bridgeWorldBuilder
);

// TODO World 2

// TODO world 3

// Fight
var fightView = new FightView(30, 50);
var fightController = new FightController(fightView, world);

// TODO Controller should have a list of worlds
var gameController = new GameController(view, world, fightController);

gameController.Start();


