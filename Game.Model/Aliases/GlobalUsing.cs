global using WorldEvents = (
    System.Timers.ElapsedEventHandler OnWorldTime,

    System.EventHandler<Game.Model.Events.WorldEventArgs<
        Game.Model.GameEntity.IGameEntity
    >> OnGoal,
    
    System.EventHandler<Game.Model.Events.WorldEventArgs<
        Game.Model.GameEntity.IHero
    >> OnGameOver,
    
    System.EventHandler<Game.Model.Events.WorldEventArgs<
        Game.Model.GameEntity.IEnemy
    >> OnFight
);
