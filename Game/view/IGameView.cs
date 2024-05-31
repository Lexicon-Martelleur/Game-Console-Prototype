using Game.constants;
using Game.model.GameArtifact;
using Game.model.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.view;

internal interface IGameView
{
    internal void DrawMap(MapHolder map);

    internal Move GetCommand();

    internal void ClearScreen();

    void WriteGameInfo(Player player);

    void WriteGameOver();
    void PrintInvalidUserOperation(string msg);
}
