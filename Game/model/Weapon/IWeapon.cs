using Game.Model.Base;

namespace Game.Model.Weapon;

internal interface IWeapon : IGameArtefact
{
    internal uint ReduceHealth { get; }
}
