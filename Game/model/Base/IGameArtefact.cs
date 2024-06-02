using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.model.Map;

namespace Game.model.Base;

internal interface IGameArtefact
{
    internal string Symbol { get; }

    internal string Name { get; }

}
