namespace DevPortal.Web.Library.Model
{
    public class SiteControllerActionNames : BaseActionNames
    {
        public static string About => SetActionName(nameof(About));

        public static string Version => SetActionName(nameof(Version));
    }
}