namespace Game.Model.Events;

public class WorldEventArgs<EventData>
{
    public EventData Data { get; }

    public WorldEventArgs(EventData data)
    {
        Data = data;
    }
}
