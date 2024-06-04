using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model.GameEntity;

internal interface IFlag: ICollectable<IGameEntity>
{
    internal uint GamePoints { get; }
}
