﻿using Game.constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model;

internal class Cliff : Terrain
{
    public string Name => "Cliff";

    public ConsoleColor Color => ColorSpectrum.Cliff;
}
