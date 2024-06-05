// Ignore Spelling: Collectable

using Timers = System.Timers;

using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Weapon;
using Game.Constant;
using Game.Model.Events;
using Game.Model.Base;
using Game.Model.GameToken;

namespace Game.Model.World;

public class WorldService(IHero hero, IWorld world) : IWorldService
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

    public event EventHandler<WorldEventArgs<IHero>>? GameOverEvent;

    public event EventHandler<WorldEventArgs<IEnemy>>? FightEvent;

    public event EventHandler<WorldEventArgs<IDiscoverableArtifact>>? PickTokenEvent;

    public IHero Hero { get => hero; }

    public IEnemy? FightingEnemy { 
        get => _fightingEnemy;
        private set => _fightingEnemy = value;
    }


    public WorldMap GetWorldSnapShot()
    {
        return world.CreateWorldSnapShot(Hero);
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
        world.Flag.Collected += _worldEventWrapper.OnGoal;

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
        world.WorldItems = world.WorldItems.Where(item => e.Data.Position != item.Position);
    }

    private void OnPickedToken(Object? source, WorldEventArgs<IDiscoverableArtifact> e)
    {
        world.WorldItems = world.WorldItems.Where(item => e.Data.Position != item.Position);
    }

    private void UpdateEnenmyPositions()
    {
        var newPossition = _oddTimeFrame ? -1 : 1;
        _oddTimeFrame = !_oddTimeFrame;
        foreach (var enemy in world.WorldItems.OfType<IEnemy>())
        {
            enemy.UpdatePosition(
                new Position(enemy.Position.x + newPossition, enemy.Position.y)
            );
        }
    }

    private IEnemy? GetFightingEnemy()
    {
        return world.WorldItems
            .OfType<IEnemy>()
            .FirstOrDefault(IsFightPosition);
    }

    private bool IsFightPosition(IDiscoverableArtifact item)
    {
        return item.Position == Hero.Position;
    }

    public IFlag GetWorldFlag()
    {
        return world.Flag;
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
        if (world.IsStoneTerrain(position) ||
            world.IsOutsideMap(position))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void UpdatePlayerHealth(Position position)
    {
        if (world.IsCliffTerrain(position) ||
            world.IsFireTerrain(position) ||
            world.IsWaterTerrain(position))
        {
            var terrain = world.GetDangerousTerrain(position);
            if (hero.Health < (terrain?.ReduceHealth() ?? 0))
            {
                hero.Health = 0;
            }
            else
            {
                hero.Health = hero.Health - terrain?.ReduceHealth() ?? 0;
            }
        }
    }

    private void PickupExistingFlag()
    {
        if (world.Flag.PickUpExistingItem(Hero, out IGameEntity entity))
        {
            Hero.Flags.Append(entity);
        }   
    }

    private void PickupExistingHeart()
    {
        world.WorldItems.ToList().ForEach(AddHealthToPlayer);
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
        var enemy = world.WorldItems
            .OfType<IEnemy>()
            .FirstOrDefault(IsFightPosition);
        if (enemy != null)
        {
            OnFight(enemy);
        }
    }

    private void OnFight(IEnemy enemy)
    {
        var e = new WorldEventArgs<IEnemy>(enemy);
        FightEvent?.Invoke(this, e);
    }

    public void RemoveEnemyFromWorld(IEnemy enemy)
    {
        world.WorldItems = world.WorldItems.Where(item =>
            enemy.Position != item.Position &&
            ArtifactIsNotEnenmy(item, enemy)
        );
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

    public string GetTerrainDescription()
    {
        return world.GetTerrainDescription();
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
        world.Flag.Collected -= _worldEventWrapper.OnGoal;
        GameOverEvent -= _worldEventWrapper.OnGameOver;
        FightEvent -= _worldEventWrapper.OnFight;
        PickTokenEvent -= _worldEventWrapper.OnGameToken;
        _worldTimer.Close();
    }
}
