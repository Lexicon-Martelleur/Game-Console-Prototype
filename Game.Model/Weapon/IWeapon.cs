using Game.Model.Base;

namespace Game.Model.Weapon;

public interface IWeapon : IGameArtefact
{
    public uint ReduceHealth { get; }
}
