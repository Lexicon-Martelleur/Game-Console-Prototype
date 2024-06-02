using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model.Weapon;

internal class Spear : IWeapon
{
    public uint ReduceHealth => 5;

    public string Symbol => "🔱";

    public string Name => "Spear";
}
