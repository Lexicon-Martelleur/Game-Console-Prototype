using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Events;

internal class WorldEventArgs<EventData>
{
    public DateTime SignalTime { get; }
    
    public EventData Data { get; }

    public WorldEventArgs(DateTime signalTime, EventData data)
    {
        SignalTime = signalTime;
        Data = data;
    }
}
