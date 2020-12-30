namespace DevPortal.Web.Library.Model
{
    public class ApplicationWikiExportControllerActionNames : BaseActionNames
    {
        public static string ExportAsWiki => SetActionName(nameof(ExportAsWiki));

        public static string ExportAsWikiFile => SetActionName(nameof(ExportAsWikiFile));
    }
}