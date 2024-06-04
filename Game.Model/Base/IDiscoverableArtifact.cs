namespace Game.Model.Base;

public interface IDiscoverableArtifact :  IGameArtifact
{
    public Position Position { get; }
}
