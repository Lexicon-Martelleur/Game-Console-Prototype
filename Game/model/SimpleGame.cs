using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model;

internal class SimpleGame(Player player) : IGame
{
    private List<GameArtifact> _artifacts = [
        player
    ];
    
    public Map Map => new Map(20, 30, GetGameArtifacts());

    public Player Player { get => player; }

    internal IEnumerable<GameArtifact> GetGameArtifacts()
    {
        return _artifacts;
    }
}
