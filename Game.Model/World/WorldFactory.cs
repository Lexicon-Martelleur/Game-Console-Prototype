using Game.Model.Constant;
using Game.Model.GameEntity;
using Game.Model.Map;

namespace Game.Model.World
{
    public class WorldFactory
    {
        public World CreateWorld()
        {
            // TODO World 2

            // TODO world 3
            var heroEntity = new Hero(1, new Position(0, 0));
            var flagBridgeGameWorld = new Flag(2, new Position(48, 28), 100);
            var antOneBridgeGameWorld = new Ant(3, new Position(19, 23));
            var antTwoBridgeGameWorld = new Ant(4, new Position(29, 12));
            var antThreeBridgeGameWorld = new Ant(5, new Position(39, 2));
            var antFourBridgeGameWorld = new Ant(6, new Position(3, 3));

            IEnumerable<IGameEntity> entitiesBridgeGameWorld = [
                antOneBridgeGameWorld,
                antTwoBridgeGameWorld,
                antThreeBridgeGameWorld,
                antFourBridgeGameWorld
            ];

            var bridgeWorldBuilder = new BridgeWorldBuilder(
                WorldConstant.WIDTH,
                WorldConstant.HEIGHT);

            return new World(
                heroEntity,
                flagBridgeGameWorld,
                entitiesBridgeGameWorld,
                bridgeWorldBuilder
            );
        }
    }
}
