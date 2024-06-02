using Game.model.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.GameEntity
{
    internal interface Hero : IGameEntity, Moveable, Living
    {
        IEnumerable<IWeapon> Weapons { get; }
    }
}
