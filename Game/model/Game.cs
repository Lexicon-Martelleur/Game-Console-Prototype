using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model;

internal interface IGame
{
    Player Player { get; }

    Map UpdateMap();
}
