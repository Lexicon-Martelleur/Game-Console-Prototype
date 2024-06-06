
namespace Game.Model.World;

public abstract class BaseWorldFactory : IWorldFactory
{
    private HashSet<uint> _gameEntityIds = [];

    public abstract WorldService CreateWorldService();

    protected uint CreateID()
    {
        Random random = new();
        uint id;
        do
        {
            id = (uint)random.Next(1, int.MaxValue);
        } while (!_gameEntityIds.Add(id));
        return id;
    }
}
