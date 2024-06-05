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

// TODO! World service should contain a stack of worlds. 
// public Stack<IWorld> Worlds { get; set; }
public class WorldService(IHero hero, Stack<IWorld> worlds) : IWorldService
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

    public IHero Hero { get => hero; }

    // TODO! Use when player reach goal and there exist more worlds
    public IWorld CurrentWorld {
        get => worlds.Peek();
    }

    public event EventHandler<WorldEventArgs<IHero>>? GameOverEvent;

    public event EventHandler<WorldEventArgs<IEnemy>>? FightEvent;

    public event EventHandler<WorldEventArgs<IDiscoverableArtifact>>? PickTokenEvent;

    public IEnemy? FightingEnemy { 
        get => _fightingEnemy;
        private set => _fightingEnemy = value;
    }


    public WorldMap? GetWorldSnapShot()
    {
        if (worlds.Count == 0)
        {
            return null;
        }
        return CurrentWorld.CreateWorldSnapShot(Hero);
    }

    // TODO! Clean up method use Wrapper methods instead
    // of anonymous functions, e.g, OnWorldTimeWrapper, OnGoalWrapper, etc...
    public void InitWorld(WorldEvents worldEvents)
    {
        // _worldEventWrapper = worldEvents; // Works ???
        _worldEventWrapper.OnWorldTime = (source, e) =>
        {
            UpdateEnenmyPositions();
            FightingEnemy = GetFightingEnemy();
            worldEvents.OnWorldTime(source, e); // TODO! How to pass this? 
        };
        _worldTimer.Elapsed += _worldEventWrapper.OnWorldTime;
        _worldTimer.AutoReset = true;
        _worldTimer.Enabled = true;


        // TODO! Add update Stack<IWorld> (POP Worlds) here which
        // will lead to a new world
        _worldEventWrapper.OnGoal = (source, e) =>
        {
            OnFlagPicked(source, e);
            Hero.UpdatePosition(new Position(1, 1)); // TODO Move to world type; 
            var prewWorld = worlds.Pop(); // TODO Send prevWorld as event args after re-factoring.
            if (worlds.Count == 0)
            {
                CloseWorld();
            }
            else
            {
                worldEvents.OnGoal(source, e); // TODO! How to pass this?
            }
        };
        CurrentWorld.Flag.Collected += _worldEventWrapper.OnGoal;

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
        CurrentWorld.WorldItems = CurrentWorld.WorldItems
            .Where(item => e.Data.Position != item.Position);
    }

    private void OnPickedToken(Object? source, WorldEventArgs<IDiscoverableArtifact> e)
    {
        CurrentWorld.WorldItems = CurrentWorld.WorldItems
            .Where(item => e.Data.Position != item.Position);
    }

    private void UpdateEnenmyPositions()
    {
        var newPossition = _oddTimeFrame ? -1 : 1;
        _oddTimeFrame = !_oddTimeFrame;
        foreach (var enemy in CurrentWorld.WorldItems.OfType<IEnemy>())
        {
            enemy.UpdatePosition(
                new Position(enemy.Position.x + newPossition, enemy.Position.y)
            );
        }
    }

    private IEnemy? GetFightingEnemy()
    {
        return CurrentWorld.WorldItems
            .OfType<IEnemy>()
            .FirstOrDefault(IsFightPosition);
    }

    private bool IsFightPosition(IDiscoverableArtifact item)
    {
        return item.Position == Hero.Position;
    }

    public string GetGoalMessage()
    {
        return $"{CurrentWorld.Symbol} {CurrentWorld.Name} task: Take the flag " +
            $"{CurrentWorld.Flag.Symbol} at " +
            $"[{CurrentWorld.Flag.Position.x}, {CurrentWorld.Flag.Position.y}] " +
            $"to win";
    }

    public void MovePlayerToNextPosition(Move move)
    {
        var nextPos = NextPlayerPosition(move);
        Hero.UpdatePosition(nextPos);
        UpdatePlayerHealth(nextPos);
        FightingEnemy = GetFightingEnemy(); // TODO! Event?
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
        if (CurrentWorld.IsStoneTerrain(position) ||
            CurrentWorld.IsOutsideMap(position))
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
        if (CurrentWorld.IsCliffTerrain(position) ||
            CurrentWorld.IsFireTerrain(position) ||
            CurrentWorld.IsWaterTerrain(position))
        {
            var terrain = CurrentWorld.GetDangerousTerrain(position);
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
        if (CurrentWorld.Flag.PickUpExistingItem(Hero, out IGameEntity entity))
        {
            Hero.Flags.Append(entity);
        }   
    }

    private void PickupExistingHeart()
    {
        CurrentWorld.WorldItems.ToList().ForEach(AddHealthToPlayer);
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
        var enemy = CurrentWorld.WorldItems
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
        CurrentWorld.WorldItems = CurrentWorld.WorldItems.Where(item =>
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
        return CurrentWorld.GetTerrainDescription();
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
        CurrentWorld.Flag.Collected -= _worldEventWrapper.OnGoal;
        GameOverEvent -= _worldEventWrapper.OnGameOver;
        FightEvent -= _worldEventWrapper.OnFight;
        PickTokenEvent -= _worldEventWrapper.OnGameToken;
        _worldTimer.Close();
    }
}
