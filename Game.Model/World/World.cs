// Ignore Spelling: Collectable

using Timers = System.Timers;

using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Weapon;
using Game.Model.Terrain;
using Game.Constant;
using Game.Model.Events;
using Game.Model.Base;
using Game.Model.GameToken;

namespace Game.Model.World;

// TODO Rename to WorldService
// TODO Move Hero, Flag and GameEntities to Builder type and rename that
// type to World. Abstract functionality that controller may need.
public class World(
    IHero hero,
    IFlag flag,
    IEnumerable<IDiscoverableArtifact> worldItems,
    IWorldBuilder worldBuilder) : IWorld
{
    private bool _oddTimeFrame = false;

    private IEnemy? _fightingEnemy = null;

    private Timers.Timer _worldTimer = new Timers.Timer(1000);

    private WorldEvents _worldEventWrapper = (
        OnWorldTime: (source, e) => { },
        OnGoal: (source, e) => { },
        OnGameOver: (source, e) => { },
        OnFight: (source, e) => { },
        OnGameToken: (source, e) => { }
    );

    private WorldMap? _worldMap;

    public event EventHandler<WorldEventArgs<IHero>>? GameOverEvent;

    public event EventHandler<WorldEventArgs<IEnemy>>? FightEvent;

    public event EventHandler<WorldEventArgs<IDiscoverableArtifact>>? PickTokenEvent;

    public IHero Hero { get => hero; }

    public IFlag Flag { get => flag; }

    public IEnemy? FightingEnemy { 
        get => _fightingEnemy;
        private set => _fightingEnemy = value;
    }

    public IEnumerable<IDiscoverableArtifact> WorldItems {
        get => worldItems.Append(hero).Append(flag);
        set => worldItems = value;
    }

    public WorldMap GetWorldSnapShot()
    {
        _worldMap = worldBuilder.CreateWorldSnapShot(
            worldItems.Append(Hero).Append(flag)
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
    public void RemoveEnemyFromWorld(IEnemy enemy)
    {
        var newWorldItems = new List<IDiscoverableArtifact>();
        foreach (var item in WorldItems)
        {
            if (ArtifactIsNotEnenmy(item, enemy))
            {
                newWorldItems.Add(item);
            }
        };
        WorldItems = newWorldItems;
        FightingEnemy = null;
    }

    private bool ArtifactIsNotEnenmy(IDiscoverableArtifact item, IEnemy enemy)
    {
        var isNotEnenmy = false;
        if (item is IGameEntity)
        {
            var entity = (IGameEntity)item;
            if (entity.Id != enemy.Id)
            {
                isNotEnenmy = true;
            }
        }
        else
        {
            isNotEnenmy = true;
        }
        return isNotEnenmy;
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

    public IDangerousTerrain? GetDangerousTerrain(Position position)
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

    public void InitWorld(WorldEvents worldEvents)
    {
        _worldEventWrapper.OnWorldTime = (source, e) =>
        {
            UpdateEnenmyPositions();
            FightingEnemy = GetFightingEnemy();
            worldEvents.OnWorldTime(source, e);
        };
        _worldTimer.Elapsed += _worldEventWrapper.OnWorldTime;
        _worldTimer.AutoReset = true;
        _worldTimer.Enabled = true;


        // TODO Add update builder here which
        // will lead to a new world
        _worldEventWrapper.OnGoal = (source, e) =>
        {
            OnFlagPicked(source, e);
            worldEvents.OnGoal(source, e);
            CloseWorld();
        };
        Flag.Collected += _worldEventWrapper.OnGoal;

        _worldEventWrapper.OnGameOver = (source, e) =>
        {
            worldEvents.OnGameOver(source, e);
            CloseWorld();
        };
        GameOverEvent += _worldEventWrapper.OnGameOver;

        _worldEventWrapper.OnFight = worldEvents.OnFight;
        FightEvent += _worldEventWrapper.OnFight;

        _worldEventWrapper.OnGameToken = (source, e) =>
        {
            OnPickedToken(source, e);
            worldEvents.OnGameToken(source, e);
        };
        PickTokenEvent += _worldEventWrapper.OnGameToken;
    }

    private void OnFlagPicked(Object? source, WorldEventArgs<IGameEntity> e)
    {
        Hero.Flags.Append(e.Data);
    }

    private void OnPickedToken(Object? source, WorldEventArgs<IDiscoverableArtifact> e)
    {
        var newWorldItems = new List<IDiscoverableArtifact>();
        foreach (var item in WorldItems)
        {
            if (e.Data.Position != item.Position)
            {
                newWorldItems.Add(item);
            }

        }
        WorldItems = newWorldItems;
    }

    private void UpdateEnenmyPositions()
    {
        var newPossition = _oddTimeFrame ? -1 : 1;
        _oddTimeFrame = !_oddTimeFrame;
        foreach (IDiscoverableArtifact item in worldItems)
        {
            if (item is IEnemy)
            {
                (item as IEnemy)?.UpdatePosition(
                    new Position(item.Position.x + newPossition, item.Position.y)
                );
            }
        }
    }

    private IEnemy? GetFightingEnemy()
    {
        IEnemy? enemy = null;
        foreach (IDiscoverableArtifact item in WorldItems)
        {
            if (item is IEnemy && IsFightPosition(item))
            {
                enemy = item as IEnemy;
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
        PickupExistingHeart();
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
        if (flag.PickUpExistingItem(Hero, out IGameEntity entity))
        {
            Hero.Flags.Append(entity);
        }   
    }

    public void PickupExistingHeart()
    {
        foreach (var item in WorldItems)
        {
            AddHealthToPlayer(item);
        }
    }

    private void AddHealthToPlayer(IDiscoverableArtifact item)
    {
        if (item is IHeart)
        {
            var heart = item as IHeart;
            if (heart != null && 
                heart.PickUpExistingItem(Hero, out IDiscoverableArtifact artifact))
            {
                var eventArgs = new WorldEventArgs<IDiscoverableArtifact>(artifact);
                PickTokenEvent?.Invoke(this, eventArgs);
            }
        }
    }

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
        GameOverEvent?.Invoke(this, e);
        CloseWorld();
    }

    private void IsFight()
    {
        foreach (IDiscoverableArtifact item in WorldItems)
        {
            if (item is IEnemy && IsFightPosition(item))
            {
                OnFight((IEnemy)item);
                break;
            }
        };
    }

    private bool IsFightPosition(IDiscoverableArtifact item)
    {
        return item.Position == Hero.Position;
    }

    private void OnFight(IEnemy enemy)
    {
        var e = new WorldEventArgs<IEnemy>(enemy);
        FightEvent?.Invoke(this, e);
    }

    public bool IsFightOver(IHero player, IEnemy enemy)
    {
        return player.Health == 0 || enemy.Health == 0;
    }

    public bool IsHeroDead()
    {
        return Hero.Health == 0;
    }

    public void CloseWorld()
    {
        _worldTimer.Elapsed -= _worldEventWrapper.OnWorldTime;
        Flag.Collected -= _worldEventWrapper.OnGoal;
        GameOverEvent -= _worldEventWrapper.OnGameOver;
        FightEvent -= _worldEventWrapper.OnFight;
        PickTokenEvent -= _worldEventWrapper.OnGameToken;
        _worldTimer.Close();
    }
}
