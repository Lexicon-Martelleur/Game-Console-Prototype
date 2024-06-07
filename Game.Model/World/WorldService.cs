// Ignore Spelling: Collectable

using Timers = System.Timers;

using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Weapon;
using Game.Constant;
using Game.Model.Events;
using Game.Model.Base;
using Game.Model.GameToken;
using Game.Utility;

namespace Game.Model.World;

public class WorldService(IHero hero, Stack<IWorld> worlds) : IWorldService
{
    private bool _oddTimeFrame = false;

    private IEnemy? _fightingEnemy = null;

    private bool _isWorldClosed = true;

    private Timers.Timer _worldTimer = new Timers.Timer(1000);

    private WorldEvents _worldEvents;

    private WorldEvents _worldEventsWrapper;

    public IHero Hero => hero;

    public IWorld CurrentWorld => worlds.Peek();

    public IEnemy? FightingEnemy
    {
        get => _fightingEnemy;
        private set => _fightingEnemy = value;
    }

    public event EventHandler<WorldEventArgs<IHero>>? GameOverEvent;

    public event EventHandler<WorldEventArgs<IEnemy>>? FightStartEvent;

    public event EventHandler<WorldEventArgs<(bool IsHeroDead, IHero Hero)>>? FightStopEvent;

    public event EventHandler<WorldEventArgs<IDiscoverableArtifact>>? CollectTokenEvent;

    public event EventHandler<WorldEventArgs<IGameEntity>>? PickFlagEvent;

    public event EventHandler<WorldEventArgs<Position>>? InvalidMoveEvent;

    public WorldMap? GetWorldSnapShot()
    {
        if (worlds.Count == 0)
        {
            return null;
        }
        return CurrentWorld.CreateWorldSnapShot(Hero);
    }

    public void InitWorld(WorldEvents worldEvents)
    {
        _worldEvents = worldEvents;
        
        _worldEventsWrapper.OnWorldTime = OnWorldTimeWrapper;
        _worldTimer.Elapsed += _worldEventsWrapper.OnWorldTime;
        _worldTimer.AutoReset = true;
        _worldTimer.Enabled = true;

        _worldEventsWrapper.OnGoal = OnGoalWrapper;
        PickFlagEvent += _worldEventsWrapper.OnGoal;

        _worldEventsWrapper.OnGameOver = OnGameOverWrapper;
        GameOverEvent += _worldEventsWrapper.OnGameOver;

        _worldEventsWrapper.OnFightStart = worldEvents.OnFightStart;
        FightStartEvent += _worldEventsWrapper.OnFightStart;

        _worldEventsWrapper.OnFightStop = worldEvents.OnFightStop;
        FightStopEvent += _worldEventsWrapper.OnFightStop;

        _worldEventsWrapper.OnGameToken = OnGameTokenWrapper;
        CollectTokenEvent += _worldEventsWrapper.OnGameToken;

        _worldEventsWrapper.OnInvalidMove = worldEvents.OnInvalidMove;
        InvalidMoveEvent += _worldEventsWrapper.OnInvalidMove;

        _isWorldClosed = false;
    }

    private void OnWorldTimeWrapper(object? source, Timers.ElapsedEventArgs e)
    {
        UpdateEnenmyPositions();
        FightingEnemy = GetFightingEnemy();
        _worldEvents.OnWorldTime(source, e);
    }

    private IEnemy? GetFightingEnemy()
    {
        return CurrentWorld.WorldItems
            .OfType<IEnemy>()
            .FirstOrDefault(IsFightPosition);
    }

    private void UpdateEnenmyPositions()
    {
        var newPossition = _oddTimeFrame ? -1 : 1;
        _oddTimeFrame = !_oddTimeFrame;
        foreach (var enemy in CurrentWorld.WorldItems.OfType<IEnemy>())
        {
            enemy.UpdatePosition(GetNewRandomPostion(enemy.Position));
        }
    }

    private void OnGoalWrapper(object? source, WorldEventArgs<IGameEntity> e)
    {
        Hero.Flags.Append(e.Data);
        CurrentWorld.WorldItems = CurrentWorld.WorldItems
            .Where(item => e.Data.Position != item.Position);

        if (worlds.Count == 1)
        {
            _worldEvents.OnGoal(source, e);
            var prewWorld = worlds.Pop();
            CloseWorld();
        }
        else
        {
            _worldEvents.OnGoal(source, e);
            var prewWorld = worlds.Pop();
            Hero.UpdatePosition(new Position(0, 0)); // TODO Move to WorldRef type;
            OnNewWorld(prewWorld);
        }
    }

    private void OnNewWorld(IWorld prevWorld)
    {
        var e = new WorldEventArgs<(
            IWorld PrevWorld, IWorld NewWorld)
        >((PrevWorld: prevWorld, NewWorld: CurrentWorld));
        _worldEvents.OnNewWorld(this, e);
    }

    private void OnGameOverWrapper(object? source, WorldEventArgs<IHero> e)
    {
        _worldEvents.OnGameOver(source, e);
        CloseWorld();
    }

    private void OnGameTokenWrapper(object? source, WorldEventArgs<IDiscoverableArtifact> e)
    {
        CurrentWorld.WorldItems = CurrentWorld.WorldItems
            .Where(item => e.Data.Position != item.Position);
        _worldEvents.OnGameToken(source, e);
    }

    private Position GetNewRandomPostion(Position prevPosition)
    {
        Position nextPosition = prevPosition;
        var isValidPos = false;
        var neigbours = GetNeighbourPositions(prevPosition);
        foreach (var position in neigbours)
        {
            nextPosition = position;
            isValidPos = IsValidPosition(nextPosition) &&
                CurrentWorld.GetDangerousTerrain(nextPosition) == null;
            if (isValidPos) { break; }
        }
        return nextPosition;
    }

    private List<Position> GetNeighbourPositions(Position pos)
    {
        List<Position> neigbours = [
            new Position(pos.x, pos.y - 1),
            new Position(pos.x + 1, pos.y - 1),
            new Position(pos.x + 1, pos.y),
            new Position(pos.x + 1, pos.y + 1),
            new Position(pos.x, pos.y + 1),
            new Position(pos.x - 1, pos.y + 1),
            new Position(pos.x - 1, pos.y),
            new Position(pos.x - 1, pos.y - 1),
        ];
        neigbours.Shuffle();
        return neigbours;
    }

    private bool IsFightPosition(IDiscoverableArtifact item)
    {
        return item.Position == Hero.Position;
    }

    public string GetGoalMessage()
    {
        return $"{CurrentWorld.Symbol} {CurrentWorld.Name}: " +
            $"Take the flag {CurrentWorld.Flag.Symbol} at " +
            $"map {CurrentWorld.Map?.Symbol ?? ""} coordinates "+
            $"[{CurrentWorld.Flag.Position.x}, {CurrentWorld.Flag.Position.y}] " +
            $"to win";
    }

    public void MoveHeroToNextPosition(Move move)
    {
        if (_isWorldClosed)
        {
            return;
        }

        if (IsNextPlayerPosition(out Position nextPos, move))
        {
            Hero.UpdatePosition(nextPos);
            UpdatePlayerHealth(nextPos);
            CheckIsGameOver();
            FightingEnemy = GetFightingEnemy();
            IsFight();
            PickupExistingHeart();
            PickupExistingFlag();
        }
        else 
        {
            OnInvalidMove(nextPos);
        }
    }

    private bool IsNextPlayerPosition(out Position nextPos, Move move)
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
        nextPos = new Position(nextX, nextY);
        return IsValidPosition(nextPos);
    }

    private bool IsValidPosition(Position position)
    {
        return !(
            CurrentWorld.IsStoneTerrain(position) ||
            CurrentWorld.IsOutsideMap(position)
        );
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

    private void PickupExistingHeart()
    {
        CurrentWorld.WorldItems
            .OfType<IHeart>()
            .ToList()
            .ForEach(AddHealthToPlayer);
    }

    private void AddHealthToPlayer(IHeart heart)
    {
        if (heart.IsCollectible(Hero, out IDiscoverableArtifact collectedHeart))
        {
            OnHeartCollected(heart, collectedHeart);
        } 
    }

    private void OnHeartCollected(IHeart heart, IDiscoverableArtifact collectedHeart)
    {
        Hero.Health = Hero.Health + heart.HealthInjection;
        var eventArgs = new WorldEventArgs<IDiscoverableArtifact>(collectedHeart);
        CollectTokenEvent?.Invoke(this, eventArgs);
    }

    private void PickupExistingFlag()
    {
        if (CurrentWorld.Flag.IsCollectible(Hero, out IGameEntity discoveredFlag))
        {
            OnFlagCollected(discoveredFlag);
        }
    }

    private void OnFlagCollected(IGameEntity collectedFlag)
    {
        Hero.Flags.Append(collectedFlag);
        var eventArgs = new WorldEventArgs<IGameEntity>(collectedFlag);
        PickFlagEvent?.Invoke(this, eventArgs);
    }

    private void OnInvalidMove(Position invalidPos)
    {
        var eventArgs = new WorldEventArgs<Position>(invalidPos);
        InvalidMoveEvent?.Invoke(this, eventArgs);
    }

    private void CheckIsGameOver()
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
            OnFightStart(enemy);
        }
    }

    private void OnFightStart(IEnemy enemy)
    {
        var e = new WorldEventArgs<IEnemy>(enemy);
        FightStartEvent?.Invoke(this, e);
    }

    public void RemoveDeadCreatures(IEnemy enemy)
    {
        if (!IsHeroDead()) 
        {
            CurrentWorld.WorldItems = CurrentWorld.WorldItems.Where(item =>
                enemy.Position != item.Position &&
                ArtifactIsNotEnenmy(item, enemy)
            );
            FightingEnemy = null;
        }
        OnFightStop();
    }

    private bool IsHeroDead()
    {
        return Hero.Health == 0;
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

    public void UpdateCreatureHealth(ICreature entity, IWeapon weapon)
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

    private void OnFightStop()
    {
        var eventArgs = new WorldEventArgs<(
            bool IsHeroDead, IHero Hero
        )>((
            IsHeroDead: IsHeroDead(),
            Hero
        ));

        FightStopEvent?.Invoke(this, eventArgs);
    }

    public void CloseWorld()
    {
        _worldTimer.Elapsed -= _worldEventsWrapper.OnWorldTime;
        GameOverEvent -= _worldEventsWrapper.OnGameOver;
        FightStartEvent -= _worldEventsWrapper.OnFightStart;
        CollectTokenEvent -= _worldEventsWrapper.OnGameToken;
        FightStopEvent -= _worldEventsWrapper.OnFightStop;
        PickFlagEvent -= _worldEventsWrapper.OnGoal;
        _worldTimer.Close();

        _isWorldClosed = true;
    }
}
