using Game.model.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.GameEntity
{
    internal interface Enemy : IGameEntity, Moveable, Living
    {
        IWeapon Weapon { get; }
    }

}
