namespace DevPortal.Web.Library.Model
{
    public class BaseActionNames
    {
        public static string SetActionName(string actionName)
        {
            return actionName.ToLowerInvariant();
        }

        public static string Index => SetActionName(nameof(Index));

        public static string Detail => SetActionName(nameof(Detail));

        public static string Add => SetActionName(nameof(Add));

        public static string Edit => SetActionName(nameof(Edit));

        public static string Delete => SetActionName(nameof(Delete));
    }
}