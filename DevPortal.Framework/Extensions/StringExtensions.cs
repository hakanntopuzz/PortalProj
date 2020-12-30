namespace DevPortal.Framework.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveLineBreaks(this string lines)
        {
            return lines.Replace("\r", "").Replace("\n", "");
        }
    }
}