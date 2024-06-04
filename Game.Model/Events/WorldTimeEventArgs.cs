namespace Game.Model.Events;

public class WorldTimeEventArgs<EventData>
{
    public DateTime SignalTime { get; }

    public EventData Data { get; }

    public WorldTimeEventArgs(DateTime signalTime, EventData data)
    {
        SignalTime = signalTime;
        Data = data;
    }
}
