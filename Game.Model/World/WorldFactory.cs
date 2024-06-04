using Game.Model.Base;
using Game.Model.Constant;
using Game.Model.GameEntity;
using Game.Model.GameToken;

namespace Game.Model.World
{
    public class WorldFactory
    {
        public World CreateWorld()
        {
            // TODO World 2

            // TODO world 3
            var heroEntity = new Hero(1, new Position(0, 0));
            
            uint gamePointsFlagBridgeWorld = 100;
            var flagBridgeGameWorld = new Flag(2, new Position(48, 28), gamePointsFlagBridgeWorld);
            
            var antOneBridgeGameWorld = new Ant(3, new Position(19, 23));
            var antTwoBridgeGameWorld = new Ant(4, new Position(29, 12));
            var antThreeBridgeGameWorld = new Ant(5, new Position(39, 2));
            var antFourBridgeGameWorld = new Ant(6, new Position(3, 3));
            var heartOne = new Heart(new Position(5, 5));
            var heartTwo = new Heart(new Position(6, 6));
            var heartThree = new Heart(new Position(7, 7));

            IEnumerable<IDiscoverableArtifact> bridgeGameWorldItems = [
                antOneBridgeGameWorld,
                antTwoBridgeGameWorld,
                antThreeBridgeGameWorld,
                antFourBridgeGameWorld,
                heartOne,
                heartTwo,
                heartThree
            ];

            var bridgeWorldBuilder = new BridgeWorldBuilder(
                WorldConstant.WIDTH,
                WorldConstant.HEIGHT);

            return new World(
                heroEntity,
                flagBridgeGameWorld,
                bridgeGameWorldItems,
                bridgeWorldBuilder
            );
        }
    }
}
