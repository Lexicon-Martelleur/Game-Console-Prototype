using Game.model.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.GameEntity
{
    internal interface IEnemy : IGameEntity, IMoveable, ILiving
    {
        IWeapon Weapon { get; }
    }

}
