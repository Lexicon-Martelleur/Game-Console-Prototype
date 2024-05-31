using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model;

internal class SimpleGame(Player player) : IGame
{
    private readonly int _height = 30;
    private readonly int _width = 50;
    private List<GameArtifact> _artifacts = [
        player
    ];
    
    public Player Player { get => player; }

    public Map UpdateMap()
    {
        return new Map(_height, _width, GetGameArtifacts());
    }

    internal IEnumerable<GameArtifact> GetGameArtifacts()
    {
        return _artifacts;
    }
}
