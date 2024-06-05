namespace Game.Utility;

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        Random randomGenerator = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = randomGenerator.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
