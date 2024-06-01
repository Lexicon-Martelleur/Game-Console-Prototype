using Game.model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.terrain;

internal interface ITerrain : IGameArtefact
{
    internal ConsoleColor Color { get; }
}
