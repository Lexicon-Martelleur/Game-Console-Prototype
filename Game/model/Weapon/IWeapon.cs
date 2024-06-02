using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.model.Base;

namespace Game.model.Weapon;

internal interface IWeapon : IGameArtefact
{
    uint ReduceHealth { get; }
}
