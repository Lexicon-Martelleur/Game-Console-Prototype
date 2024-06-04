namespace Game.Model.Terrain;

public interface IDangerousTerrain : ITerrain
{
    public uint ReduceHealth();
}
