namespace Game.Utility;

public static class ConsoleExtensions
{
    public static void WaitForEnter(this TextReader reader)
    {
        ConsoleKey key;
        do
        {
            key = Console.ReadKey(true).Key;
        } while (key != ConsoleKey.Enter);
    }
}
