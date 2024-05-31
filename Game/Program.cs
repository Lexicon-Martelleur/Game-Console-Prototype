
using Game.controller;
using Game.model;
using Game.view;

var view = new GameView();
var game = new SimpleGame();

var gameController = new GameController(view, game);

gameController.Start();
