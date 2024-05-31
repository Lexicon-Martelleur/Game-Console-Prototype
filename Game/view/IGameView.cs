using Game.constants;
using Game.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.view;

internal interface IGameView
{
    internal void DrawMap(Map map);

    internal Move GetCommand();

    internal void ClearScreen();
}
