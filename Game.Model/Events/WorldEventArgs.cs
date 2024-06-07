namespace Game.Model.Events;

public class WorldEventArgs<EventData> : EventArgs
{
    public EventData Data { get; }

    public WorldEventArgs(EventData data) : base()
    {
        Data = data;
    }
}
