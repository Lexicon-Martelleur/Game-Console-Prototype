using Game.Model.Base;
using Game.Model.GameEntity;
using Game.Model.GameToken;

namespace Game.Model.World;

public class WorldFactory
{
    private HashSet<uint> _gameEntityIds = [];
    
    public WorldService CreateWorldService()
    {
        var heroEntity = new Hero(CreateID(), new Position(0, 0));
        
        var easyBridgeWorld = GetEasyBridgeWorld();
        var mediumBridgeWorld = GetMediumBridgeWorld();
        var impossibleBridgeWorld = GetImpossibleBridgeWorld();

        Stack<IWorld> worlds = [];
        worlds.Push(impossibleBridgeWorld);
        worlds.Push(mediumBridgeWorld);
        worlds.Push(easyBridgeWorld);

        return new WorldService(
            heroEntity,
            worlds
        );
    }

    private uint CreateID()
    {
        Random random = new();
        uint id;
        do
        {
            id = (uint)random.Next(1, int.MaxValue);
        } while (!_gameEntityIds.Add(id));
        return id;
    }

    private IWorld GetEasyBridgeWorld()
    {
        uint gamePointsFlagBridgeWorld = 100;
        var flagBridgeGameWorld = new Flag(
            CreateID(),
            new Position(48, 28),
            gamePointsFlagBridgeWorld);

        IEnumerable<IDiscoverableArtifact> bridgeGameWorldItems = [
            new Ant(CreateID(), new Position(19, 23)),
            new Ant(CreateID(), new Position(29, 12)),
            new Ant(CreateID(), new Position(39, 2)),
            new Ant(CreateID(), new Position(3, 3)),
            new Ant(CreateID(), new Position(19, 23)),
            new Ant(CreateID(), new Position(29, 12)),
            new Ant(CreateID(), new Position(39, 2)),
            new Ant(CreateID(), new Position(3, 3)),
            new Heart(new Position(5, 5)),
            new Heart(new Position(6, 6)),
            new Heart(new Position(7, 7)),
            new Heart(new Position(8, 8)),
            new Heart(new Position(9, 9)),
            new Heart(new Position(9, 10))
        ];

        return new BridgeWorld(
            "Easy Bridge World",
            flagBridgeGameWorld,
            bridgeGameWorldItems);
    }

    private IWorld GetMediumBridgeWorld()
    {
        uint gamePointsFlagBridgeWorld = 100;
        var flagBridgeGameWorld = new Flag(
            CreateID(),
            new Position(48, 28),
            gamePointsFlagBridgeWorld);

        IEnumerable<IDiscoverableArtifact> bridgeGameWorldItems = [
            new Ant(CreateID(), new Position(19, 23)),
            new Ant(CreateID(), new Position(29, 12)),
            new Ant(CreateID(), new Position(39, 2)),
            new Ant(CreateID(), new Position(3, 3)),
            new Ant(CreateID(), new Position(19, 23)),
            new Ant(CreateID(), new Position(29, 12)),
            new Ant(CreateID(), new Position(39, 2)),
            new Ant(CreateID(), new Position(3, 3)),
            new Ant(CreateID(), new Position(19, 23)),
            new Ant(CreateID(), new Position(29, 12)),
            new Ant(CreateID(), new Position(39, 2)),
            new Ant(CreateID(), new Position(3, 3)),
            new Ant(CreateID(), new Position(19, 23)),
            new Ant(CreateID(), new Position(29, 12)),
            new Ant(CreateID(), new Position(39, 2)),
            new Ant(CreateID(), new Position(3, 3)),
            new Heart(new Position(5, 5)),
            new Heart(new Position(6, 6)),
            new Heart(new Position(7, 7))
        ];

        return new BridgeWorld(
            "Medium Bridge World",
            flagBridgeGameWorld,
            bridgeGameWorldItems);
    }

    private IWorld GetImpossibleBridgeWorld()
    {
        uint gamePointsFlagBridgeWorld = 100;
        var flagBridgeGameWorld = new Flag(
            CreateID(),
            new Position(48, 28),
            gamePointsFlagBridgeWorld);

        IEnumerable<IDiscoverableArtifact> bridgeGameWorldItems = [
            new Ant(CreateID(), new Position(19, 23)),
            new Ant(CreateID(), new Position(29, 12)),
            new Ant(CreateID(), new Position(39, 2)),
            new Ant(CreateID(), new Position(3, 3)),
            new Ant(CreateID(), new Position(19, 23)),
            new Ant(CreateID(), new Position(29, 12)),
            new Ant(CreateID(), new Position(39, 2)),
            new Ant(CreateID(), new Position(3, 3)),
            new Ant(CreateID(), new Position(19, 23)),
            new Ant(CreateID(), new Position(29, 12)),
            new Ant(CreateID(), new Position(39, 2)),
            new Ant(CreateID(), new Position(3, 3)),
            new Ant(CreateID(), new Position(19, 23)),
            new Ant(CreateID(), new Position(29, 12)),
            new Ant(CreateID(), new Position(39, 2)),
            new Ant(CreateID(), new Position(3, 3)),
            new Ant(CreateID(), new Position(19, 23)),
            new Ant(CreateID(), new Position(29, 12)),
            new Ant(CreateID(), new Position(39, 2)),
            new Ant(CreateID(), new Position(3, 3)),
            new Ant(CreateID(), new Position(19, 23)),
            new Ant(CreateID(), new Position(29, 12)),
            new Ant(CreateID(), new Position(39, 2)),
            new Ant(CreateID(), new Position(3, 3)),
            new Heart(new Position(5, 5)),
            new Heart(new Position(6, 6)),
            new Heart(new Position(7, 7))
        ];

        return new BridgeWorld(
            "Impossible Bridge World",
            flagBridgeGameWorld,
            bridgeGameWorldItems);
    }
}
