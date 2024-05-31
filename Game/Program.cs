
using Game.controller;
using Game.model;
using Game.view;

var view = new GameViewBuffer();
var player = new Player(new Position(0, 0));
var game = new BridgeGame(player);

var gameController = new GameController(view, game);

gameController.Start();
