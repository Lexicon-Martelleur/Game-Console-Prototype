namespace Game.Utility;

public static class StringExtensions
{
    public static string GetConsistentWidth(this string content, int width)
    {
        string consistentWidth = "";

        if (content.Length > width)
        {
            consistentWidth = content.Substring(0, width);
        }
        else if (content.Length < width)
        {
            consistentWidth = content.PadRight(width);
        }

        return consistentWidth;
    }
}
