using Game.Model.Base;

namespace Game.Model.Weapon;

public interface IWeapon : IGameArtifact
{
    public uint ReduceHealth { get; }
}
