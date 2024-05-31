using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.model;

internal class Player(Position initialPosition) : GameArtifact, Moveable
{
    private Position _currentPosition = initialPosition; 
    public string Symbol => "🐝";

    public string Name => "Player";

    public Position InitialPosition => initialPosition;

    public Position CurrentPosition {
        get => InitialPosition;
        set => _currentPosition = value;
    }
}
