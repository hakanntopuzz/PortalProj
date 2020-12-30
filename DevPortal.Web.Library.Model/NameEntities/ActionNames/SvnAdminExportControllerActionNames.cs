namespace DevPortal.Web.Library.Model
{
    public class SvnAdminExportControllerActionNames : BaseActionNames
    {
        public static string ExportToCsv => SetActionName(nameof(ExportToCsv));

        public static string ExportAsWiki => SetActionName(nameof(ExportAsWiki));

        public static string ExportAsWikiFile => SetActionName(nameof(ExportAsWikiFile));
    }
}