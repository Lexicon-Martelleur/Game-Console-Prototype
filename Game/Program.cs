
using Game.controller;
using Game.model;
using Game.model.GameArtifact;
using Game.model.Map;
using Game.model.World;
using Game.view;

var view = new GameView();
var player = new Player(new Position(0, 0));
var game = new BridgeGameWorld(player);

var gameController = new GameController(view, game);

gameController.Start();
