namespace Game.Model.Terrain;

internal interface IDangerousTerrain : ITerrain
{
    internal uint ReduceHealth();
}
