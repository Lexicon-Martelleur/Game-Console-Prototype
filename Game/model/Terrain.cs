using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model
{
    internal interface Terrain
    {
        string Name { get; }

        ConsoleColor Color { get; }
    }
}
