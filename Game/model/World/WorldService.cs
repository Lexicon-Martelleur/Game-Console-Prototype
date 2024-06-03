using Timers = System.Timers;

using Game.Model.GameEntity;
using Game.Model.Map;
using Game.Model.Weapon;
using Game.Model.Terrain;
using Game.constants;

namespace Game.Model.World;

internal class WorldService(
    Hero player,
    Flag flag,
    IEnumerable<IGameEntity> entities,
    IWorldBuilder worldBuilder) : IWorldService
{
    private bool _oddTimeFrame = false;

    private IEnemy? _fightingEnemy = null;

    private Timers.Timer _worldTimer = new Timers.Timer(1000);

    private Timers.ElapsedEventHandler? onWorldTimeChangeWrapper;

    private WorldMap? _worldMap;

    public Timers.Timer WorldTimer {
        get => _worldTimer;
        set => _worldTimer = value;
    }

    public Hero Player { get => player; }

    public Flag Flag { get => flag; }

    public IEnemy? FightingEnemy { 
        get => _fightingEnemy;
        private set => _fightingEnemy = value;
    }

    public IEnumerable<IGameEntity> GameEntities {
        get => entities.Append(player).Append(flag);
        private set => entities = value;
    }

    public WorldMap GetWorldSnapShot()
    {
        _worldMap = worldBuilder.CreateWorldSnapShot(
            entities.Append(Player).Append(flag)
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
            if (player.Health < (terrain?.ReduceHealth() ?? 0)) {
                player.Health = 0;
            }
            else
            {
                player.Health = player.Health - terrain?.ReduceHealth() ?? 0;
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

    public void InitWorld(Timers.ElapsedEventHandler onWorldTimeChange)
    {
        onWorldTimeChangeWrapper = (sender, e) =>
        {
            UpdateEnenmyPositions();
            FightingEnemy = GetFightingEnemy();
            onWorldTimeChange(sender, e);
        };
        WorldTimer.Elapsed += onWorldTimeChangeWrapper;
        WorldTimer.AutoReset = true;
        WorldTimer.Enabled = true;
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
        return entity.Position == Player.Position;
    }

    public void MovePlayerToNextPosition(Move move)
    {

        int nextY = Player.Position.y;
        int nextX = Player.Position.x;
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
    }

    private void UpdatePlayerPosition(Position position)
    {
        Player.UpdatePosition(position);
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

    public bool IsGameOver(out bool isGameOver)
    {
        isGameOver = Player.Health == 0;
        if (isGameOver)
        {
            CloseWorld();
        }
        return isGameOver;
    }

    public bool IsGoal(out bool isGoal)
    {
        isGoal = Player.Position == flag.Position;
        if (isGoal) {
            CloseWorld();
        }
        return isGoal;
    }

    public void CloseWorld()
    {
        WorldTimer.Elapsed -= onWorldTimeChangeWrapper;
        WorldTimer.Close();  
    }
}
