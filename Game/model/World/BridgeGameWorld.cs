﻿using Timers = System.Timers;
using Game.model.GameEntity;
using Game.model.Map;
using Game.model.terrain;
using System.Timers;

namespace Game.model.World;

internal class BridgeGameWorld(
    Player player,
    Flag flag,
    List<IGameEntity> entities) : IGameWorld
{

    private readonly int _height = 30;

    private readonly int _width = 50;

    private bool _odd = false;

    private MapHolder? _mapHolder;

    private Timers.Timer _worldTimer = new Timers.Timer(1000);

    private ElapsedEventHandler? onWorldTimeChangeWrapper;

    public Timers.Timer WorldTimer {
        get => _worldTimer;
        set => _worldTimer = value;
    }

    public Player Player { get => player; }

    public Flag Flag { get => flag; }

    public MapHolder UpdateMap()
    {
        _mapHolder = new MapHolder(_height, _width, DrawMap(GetGameEntities()));
        return _mapHolder;
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

    internal IEnumerable<IGameEntity> GetGameEntities()
    {
        return entities;
    }

    internal Cell[,] DrawMap(IEnumerable<IGameEntity> entities)
    {
        var cells = new Cell[_height, _width];
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Position position = new Position(x, y);
                ITerrain terrain = GetPositionTerrain(position);

                cells[y, x] = new Cell(
                    position,
                    terrain,
                    GetArtifactForPosition(entities, position)
                );
            }

        }
        return cells;
    }

    private ITerrain GetPositionTerrain(Position position)
    {
        if (IsCliffTerrain(position))
        {
            return new Cliff();
        }
        if (IsFireTerrain(position))
        {
            return new Fire();
        }
        if (IsWaterTerrain(position))
        {
            return new Water();
        }
        if (IsStoneTerrain(position))
        {
            return new Stone();
        }
        else
        {
            return new Grass();
        }
    }

    private bool IsStoneTerrain(Position position)
    {
        return (position.x == 10 || position.x == 11) && position.y != 5;
    }

    private bool IsFireTerrain(Position position)
    {
        return (position.x == 20 || position.x == 21) && position.y != 23;
    }

    private bool IsWaterTerrain(Position position)
    {
        return (position.x == 30 || position.x == 31) && position.y != 12;
    }

    private bool IsCliffTerrain(Position position)
    {
        return (position.x == 40 || position.x == 41) && position.y != 2;
    }

    private IGameEntity? GetArtifactForPosition(
        IEnumerable<IGameEntity> entities,
        Position position)
    {
        foreach (IGameEntity entity in entities)
        {
            if (entity.Position == position)
            {
                return entity;
            }
        }
        return null;
    }

    public void UpdatePlayerPosition(Position position)
    {
        if (!IsValidPosition(position))
        {
            throw new InvalidOperationException($"Player can not move to position [{position.x}, {position.y}]");
        }
        Player.UpdatePosition(position);
        UpdatePlayerHealth(position);
    }

    private void UpdatePlayerHealth(Position position)
    {
        if (IsCliffTerrain(position) ||
            IsFireTerrain(position) ||
            IsWaterTerrain(position))
        {
            var terrain = _mapHolder?.GetDangerousTerrain(position);
            if (player.Health < (terrain?.ReduceHealth() ?? 0)) {
                player.Health = 0;
            }
            else
            {
                player.Health = player.Health - terrain?.ReduceHealth() ?? 0;
            }
        }
    }

    private bool IsValidPosition (Position position)
    {
        if (IsStoneTerrain(position) ||
            position.x < 0 ||
            position.x >= _width ||
            position.y < 0 ||
            position.y >= _height)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void InitWorld(Timers.ElapsedEventHandler onWorldTimeChange)
    {
        onWorldTimeChangeWrapper = (sender, e) =>
        {
            UpdateWorld();
            onWorldTimeChange(sender, e);
        };
        WorldTimer.Elapsed += onWorldTimeChangeWrapper;
        WorldTimer.AutoReset = true;
        WorldTimer.Enabled = true;
    }

    private void UpdateWorld()
    {
        var newPossition = _odd ? -1 : 1;
        _odd = !_odd;
        foreach (IGameEntity entity in entities)
        {
            if (entity is Ant || entity is Froggy)
            {
                (entity as Moveable)?.UpdatePosition(
                    new Position(entity.Position.x + newPossition, entity.Position.y)
                );
            }
        }
    }

    public bool IsGameOver()
    {
        var isGameOver = Player.Health == 0;
        if (isGameOver)
        {
            WorldTimer.Elapsed -= onWorldTimeChangeWrapper;
        }
        return isGameOver;
    }

    public bool IsGoal()
    {
        var isGoal = Player.Position == flag.Position;
        if (isGoal) {
            WorldTimer.Elapsed -= onWorldTimeChangeWrapper;
        }
        return isGoal;
    }
}
