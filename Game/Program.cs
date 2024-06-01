
using Game.controller;
using Game.model.GameEntity;
using Game.model.Map;
using Game.model.World;
using Game.view;

// World 1 BridgeGameWorld
var view = new GameView();
var playerEntity = new Player(1, new Position(0, 0));
var flagBridgeGameWorld = new Flag(2, new Position(48, 28));
List<IGameEntity> entitiesBridgeGameWorld = [
    playerEntity,
    flagBridgeGameWorld
];
var world = new BridgeGameWorld(
    playerEntity,
    flagBridgeGameWorld, 
    entitiesBridgeGameWorld
);


// TODO World 2

// TODO world 3

// TODO Controller should have a list of worlds
var gameController = new GameController(view, world);

gameController.Start();


