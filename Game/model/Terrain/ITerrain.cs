using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.terrain;

internal interface ITerrain
{
    internal string Name { get; }

    internal string Symbol { get; }

    internal ConsoleColor Color { get; }
}
