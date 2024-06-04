// Ignore Spelling: Collectable

using Timers = System.Timers;

using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Weapon;
using Game.Model.Terrain;
using Game.constants;
using Game.Events;
using System;

namespace Game.Model.World;

internal class WorldService(
    Hero hero,
    Flag flag,
    IEnumerable<IGameEntity> entities,
    IWorldBuilder worldBuilder) : IWorldService
{
    private bool _oddTimeFrame = false;

    private IEnemy? _fightingEnemy = null;

    private Timers.Timer _worldTimer = new Timers.Timer(1000);

    private Timers.ElapsedEventHandler? _onWorldTimeChangeWrapper;

    private EventHandler<WorldEventArgs<IGameEntity>>? _onGoalWrapper;

    private WorldMap? _worldMap;

    public Timers.Timer WorldTimer {
        get => _worldTimer;
        set => _worldTimer = value;
    }

    public Hero Hero { get => hero; }

    public Flag Flag { get => flag; }

    public IEnemy? FightingEnemy { 
        get => _fightingEnemy;
        private set => _fightingEnemy = value;
    }

    public IEnumerable<IGameEntity> GameEntities {
        get => entities.Append(hero).Append(flag);
        private set => entities = value;
    }

    public WorldMap GetWorldSnapShot()
    {
        _worldMap = worldBuilder.CreateWorldSnapShot(
            entities.Append(Hero).Append(flag)
        );
        return _worldMap;
    }

    public string GetTerrainInfo()
    {
        var fire = new Fire();
        var water = new Water();
        var cliff = new Cliff();
        var stone = new Stone();
        return $"({stone.Symbol} = -0," +
            $" {water.Symbol} = -{water.ReduceHealth()}," +
            $" {fire.Symbol} = -{fire.ReduceHealth()}," +
            $" {cliff.Symbol} = -{cliff.ReduceHealth()})";
    }

    // TODO Implement equality check
    public void RemoveFightingEnemyFromWorld(IEnemy enemy)
    {
        var newEntitites = new List<IGameEntity>();
        foreach (var entity in GameEntities)
        {
            if (entity.Id != enemy.Id)
            {
                newEntitites.Add(entity);
            }
        };
        GameEntities = newEntitites;
        FightingEnemy = null;
    }

    private void UpdatePlayerHealth(Position position)
    {
        if (worldBuilder.IsCliffTerrain(position) ||
            worldBuilder.IsFireTerrain(position) ||
            worldBuilder.IsWaterTerrain(position))
        {
            var terrain = GetDangerousTerrain(position);
            if (hero.Health < (terrain?.ReduceHealth() ?? 0)) {
                hero.Health = 0;
            }
            else
            {
                hero.Health = hero.Health - terrain?.ReduceHealth() ?? 0;
            }
        }
    }

    internal IDangerousTerrain? GetDangerousTerrain(Position position)
    {
        Cell? findCell = null;
        var worldMapSnapShot = _worldMap ?? GetWorldSnapShot();
        foreach (Cell cell in worldMapSnapShot.Cells)
        {
            if (cell.Position == position)
            {
                findCell = cell;
                break;
            }
        }
        var terrain = findCell?.Terrain;
        return terrain as IDangerousTerrain;
    }

    public void UpdateEntityHealth(ICreature entity, IWeapon weapon)
    {
        if (entity.Health < weapon.ReduceHealth)
        {
            entity.Health = 0;
        }
        else
        {
            entity.Health = entity.Health - weapon.ReduceHealth;
        }
    }

    public void InitWorld(
        Timers.ElapsedEventHandler onWorldTimeChange,
        EventHandler<WorldEventArgs<IGameEntity>> onGoal
    )
    {
        _onWorldTimeChangeWrapper = (source, e) =>
        {
            UpdateEnenmyPositions();
            FightingEnemy = GetFightingEnemy();
            onWorldTimeChange(source, e);
        };
        WorldTimer.Elapsed += _onWorldTimeChangeWrapper;
        WorldTimer.AutoReset = true;
        WorldTimer.Enabled = true;

        //Flag.Collected += OnFlagPicked;

        _onGoalWrapper = (source, e) =>
        {
            OnFlagPicked(source, e);
            onGoal(source, e);
            CloseWorld();
        };
        Flag.Collected += _onGoalWrapper;
    }

    private void OnFlagPicked(Object? source, WorldEventArgs<IGameEntity> e)
    {
        Hero.Flags.Append(e.Data);
        //var newEntitites = new List<IGameEntity>();
        //foreach (var entity in GameEntities)
        //{
        //    if (entity.Id != e.Data.Id)
        //    {
        //        newEntitites.Add(entity);
        //    }
        //};
        //GameEntities = newEntitites;
        // CloseWorld();
    }

    private void UpdateEnenmyPositions()
    {
        var newPossition = _oddTimeFrame ? -1 : 1;
        _oddTimeFrame = !_oddTimeFrame;
        foreach (IGameEntity entity in entities)
        {
            if (entity is IEnemy)
            {
                (entity as IEnemy)?.UpdatePosition(
                    new Position(entity.Position.x + newPossition, entity.Position.y)
                );
            }
        }
    }

    private IEnemy? GetFightingEnemy()
    {
        IEnemy? enemy = null;
        foreach (IGameEntity entity in GameEntities)
        {
            if (entity is IEnemy && IsFightPosition(entity))
            {
                enemy = entity as IEnemy;
                break;
            }
        };
        return enemy;
    }

    private bool IsFightPosition(IGameEntity entity)
    {
        return entity.Position == Hero.Position;
    }

    public void MovePlayerToNextPosition(Move move)
    {

        int nextY = Hero.Position.y;
        int nextX = Hero.Position.x;
        switch (move)
        {
            case Move.UP: nextY--; break;
            case Move.RIGHT: nextX++; break;
            case Move.DOWN: nextY++; break;
            case Move.LEFT: nextX--; break;
            default: break;
        }
        var nextPos = new Position(nextX, nextY);
        if (!IsValidPosition(nextPos))
        {
            throw new InvalidOperationException(
                $"Player can not move to position [{nextPos.x}, {nextPos.y}]"
            );
        }
        UpdatePlayerPosition(nextPos);
        PickupExistingFlag();
    }

    private void UpdatePlayerPosition(Position position)
    {
        Hero.UpdatePosition(position);
        UpdatePlayerHealth(position);
        FightingEnemy = GetFightingEnemy();
    }

    private bool IsValidPosition(Position position)
    {
        if (worldBuilder.IsStoneTerrain(position) ||
            worldBuilder.IsOutsideMap(position))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void PickupExistingFlag()
    {
        if (flag.PickUpExistingEntity(Hero, out IGameEntity entity))
        {
            Hero.Flags.Append(entity);
        }   
    }

    //public void PickupExistingToken()
    //{
    //    foreach (var entity in GameEntities)
    //    {
    //        if (entity is ICollectable<IGameEntity> &&
    //            entity.Position == Hero.Position)
    //        {
    //            return entity as ICollectable<IGameEntity>;
    //        }
    //    }
    //    return null;
    //}

    public void GiveTokenToHero(ICollectable<IGameEntity> token)
    {
        Hero.Flags.Append(token);
    }

    public bool IsGameOver(out bool isGameOver)
    {
        isGameOver = Hero.Health == 0;
        if (isGameOver)
        {
            CloseWorld();
        }
        return isGameOver;
    }

    public bool IsGoal(out bool isGoal)
    {
        isGoal = Hero.Position == flag.Position;
        if (isGoal) {
            CloseWorld();
        }
        return isGoal;
    }

    public void CloseWorld()
    {
        WorldTimer.Elapsed -= _onWorldTimeChangeWrapper;
        Flag.Collected -= _onGoalWrapper;
        WorldTimer.Close();  
    }
}
