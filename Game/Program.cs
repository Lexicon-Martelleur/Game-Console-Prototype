
using Game.controller;
using Game.model;
using Game.model.GameArtifact;
using Game.model.Map;
using Game.model.World;
using Game.view;

var view = new GameView();
var player = new Player(new Position(0, 0));
var flag = new Flag(new Position(48, 28));
List<IGameArtifact> artifacts = [
    player,
    flag
];
var world = new BridgeGameWorld(player, flag, artifacts);

var gameController = new GameController(view, world);

gameController.Start();
