public static class StringExtensions
{
    public static string Substring(this string str, int begin, int end)
    {
        return str.Substring(begin, end - begin + 1);
    }
}
