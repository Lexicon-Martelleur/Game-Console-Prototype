using Game.model.Base;
using Game.model.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.GameEntity;

internal interface IGameEntity : IGameArtefact
{
    internal uint Id { get; }

    internal Position Position { get; }
}
