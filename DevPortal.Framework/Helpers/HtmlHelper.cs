namespace DevPortal.Framework.Helpers
{
    public static class HtmlHelper
    {
        public static string ToHtml(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // var rx = new System.Text.RegularExpressions.Regex(@"(\d{2}\:\d{2}\:\d{4}\s\d{2}\:\d{2}\:\d{2})\s\[\d+\]\s(ERROR)\s-\s\((.+\..+)\)\s-\s(.+)\n");
            // text = rx.Replace(text, "<font color=\"red\">$&</font>");

            text = text.Replace("ERROR -", "<b style='color:red'>ERROR -</b>");
            text = text.Replace("INFO -", "<b style='color:blue'>INFO -</b>");

            return text;
        }
    }
}