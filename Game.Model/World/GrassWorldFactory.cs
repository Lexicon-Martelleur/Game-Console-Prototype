using Game.Model.Base;
using Game.Model.GameEntity;
using Game.Model.GameToken;

namespace Game.Model.World;

public class GrassWorldFactory : BaseWorldFactory
{
    public override WorldService CreateWorldService()
    {
        var heroEntity = new Hero(CreateID(), new Position(0, 0));

        var easyGrassWorld = GetImpossibleGrassWorld();
        var mediumGrassWorld = GetImpossibleGrassWorld();
        var impossibleGrassWorld = GetImpossibleGrassWorld();

        Stack<IWorld> worlds = [];
        worlds.Push(impossibleGrassWorld);
        worlds.Push(mediumGrassWorld);
        worlds.Push(easyGrassWorld);

        return new WorldService(
            heroEntity,
            worlds
        );
    }

    private IWorld GetEasyGrassWorld()
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

        return new GrassWorld(
            "Easy Grass World",
            flagBridgeGameWorld,
            bridgeGameWorldItems);
    }

    private IWorld GetMediumGrassWorld()
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

        return new GrassWorld(
            "Medium Grass World",
            flagBridgeGameWorld,
            bridgeGameWorldItems);
    }

    private IWorld GetImpossibleGrassWorld()
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

        return new GrassWorld(
            "Impossible Grass World",
            flagBridgeGameWorld,
            bridgeGameWorldItems);
    }
}
