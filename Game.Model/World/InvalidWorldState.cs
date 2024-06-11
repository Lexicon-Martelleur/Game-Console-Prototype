namespace Game.Model.World;

internal class InvalidWorldState : InvalidOperationException
{
    internal InvalidWorldState() { }

    internal InvalidWorldState(string message) : base(message) { }

    internal InvalidWorldState(string message, Exception innerException)
        : base(message, innerException) { }
}
