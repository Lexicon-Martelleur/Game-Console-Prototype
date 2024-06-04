using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model.GameToken;

public interface IHeart : IGameToken
{
    public uint HealthInjection { get; }
}
