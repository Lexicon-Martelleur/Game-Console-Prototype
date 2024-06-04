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

internal class World(
    IHero hero,
    IFlag flag,
    IEnumerable<IGameEntity> entities,
    IWorldBuilder worldBuilder) : IWorld
{
    private bool _oddTimeFrame = false;

    private IEnemy? _fightingEnemy = null;

    private Timers.Timer _worldTimer = new Timers.Timer(1000);

    private Timers.ElapsedEventHandler? _onWorldTimeChangeWrapper;

    private EventHandler<WorldEventArgs<IGameEntity>>? _onGoalWrapper;

    private EventHandler<WorldEventArgs<IHero>>? _onGameOverWrapper;

    private EventHandler<WorldEventArgs<IEnemy>>? _onFightWrapper;

    private WorldMap? _worldMap;

    public event EventHandler<WorldEventArgs<IHero>>? GameOver;

    public event EventHandler<WorldEventArgs<IEnemy>>? Fight;

    public Timers.Timer WorldTimer {
        get => _worldTimer;
        set => _worldTimer = value;
    }

    public IHero Hero { get => hero; }

    public IFlag Flag { get => flag; }

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
        EventHandler<WorldEventArgs<IGameEntity>> onGoal,
        EventHandler<WorldEventArgs<IHero>> onGameOver,
        EventHandler<WorldEventArgs<IEnemy>> onFight
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

        
        // TODO Add update builder here which
        // will lead to a new world
        _onGoalWrapper = (source, e) =>
        {
            OnFlagPicked(source, e);
            onGoal(source, e);
            CloseWorld();
        };
        Flag.Collected += _onGoalWrapper;

        _onGameOverWrapper = (source, e) =>
        {
            onGameOver(source, e);
            CloseWorld();
        };
        GameOver += _onGameOverWrapper;

        _onFightWrapper = (source, e) =>
        {
            onFight(source, e);
        };
        Fight += _onFightWrapper;
    }

    private void OnFlagPicked(Object? source, WorldEventArgs<IGameEntity> e)
    {
        Hero.Flags.Append(e.Data);
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

    public void MovePlayerToNextPosition(Move move)
    {
        var nextPos = NextPlayerPosition(move);
        Hero.UpdatePosition(nextPos);
        UpdatePlayerHealth(nextPos);
        FightingEnemy = GetFightingEnemy(); // TODO Event?
        IsGameOver();
        IsFight();
        PickupExistingFlag();
    }

    private Position NextPlayerPosition(Move move)
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
        return nextPos;
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

    private void IsGameOver()
    {
        var isGameOver = Hero.Health == 0;
        if (isGameOver)
        {
            OnGameOver();
        }
    }

    private void OnGameOver()
    {
        var e = new WorldEventArgs<IHero>(Hero);
        GameOver?.Invoke(this, e);
        CloseWorld();
    }

    private void IsFight()
    {
        foreach (IGameEntity entity in GameEntities)
        {
            if (entity is IEnemy && IsFightPosition(entity))
            {
                OnFight((IEnemy)entity);
                break;
            }
        };
    }

    private bool IsFightPosition(IGameEntity entity)
    {
        return entity.Position == Hero.Position;
    }

    private void OnFight(IEnemy enemy)
    {
        var e = new WorldEventArgs<IEnemy>(enemy);
        Fight?.Invoke(this, e);
    }

    public bool IsHeroDead()
    {
        return Hero.Health == 0;
    }

    public void CloseWorld()
    {
        WorldTimer.Elapsed -= _onWorldTimeChangeWrapper;
        Flag.Collected -= _onGoalWrapper;
        GameOver -= _onGameOverWrapper;
        Fight -= _onFightWrapper;
        WorldTimer.Close();
    }
}
