namespace Game.Events;

internal class WorldEventArgs<EventData>
{
    public EventData Data { get; }

    public WorldEventArgs(EventData data)
    {
        Data = data;
    }
}
